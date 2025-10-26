using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

namespace Select
{
    public class SelectionManager : MonoBehaviour
    {
        [SerializeField] InputReader inputReader;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private PieceManager pieceManager;
        public ITileSelector tileSelector { get; private set; }
        private IPieceSelector pieceSelector;
        private HighLightTile highLightTile;

        private void Awake()
        {
            tileSelector = GetComponent<TileSelector>();
            pieceSelector = GetComponent<HoverPieceSelector>();
            highLightTile = GetComponent<HighLightTile>();
        }

        private void OnEnable()
        {
            inputReader.OnClickEvent += Select;
        }

        private void OnDisable()
        {
            inputReader.OnClickEvent -= Select;
        }

        public async void Select(Vector2 mousePosition)
        {
            var ray = Camera.main.ScreenPointToRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Tile") && hit.collider.TryGetComponent<Tile>(out Tile tile))
                {
                    tileSelector.SelectTile(tile);
                }
                else if (hit.collider.CompareTag("Piece"))
                {
                    var piece = hit.collider.GetComponent<Piece>();
                    if (piece && piece.type == pieceManager.currentPieceType)
                    {
                        await SelectPieceAndShowTiles(piece);
                    }
                }
            }
        }

        private async UniTask SelectPieceAndShowTiles(Piece piece)
        {
            await pieceSelector.SelectPiece(piece);
            highLightTile.ShowOptionalTilesSelection();
        }
    }
}