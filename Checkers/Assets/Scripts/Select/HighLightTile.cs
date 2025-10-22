using System.Collections.Generic;
using DefaultNamespace;
using Moves;
using UnityEngine;

namespace Select
{
    public class HighLightTile : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private PieceManager pieceManager;
        private List<Moves.Move> optionalTiles = new List<Moves.Move>();
        private MoveOptionsProvider moveOptionsProvider;
        private TileSelector tileSelector;

        public void Awake()
        {
            moveOptionsProvider = new MoveOptionsProvider(gridManager, gameManager, pieceManager);
            tileSelector = GetComponent<TileSelector>();
        }

        public void ShowOptionalTilesSelection()
        {
            if (gameManager.selectedPiece == null) return;

            if (tileSelector != null && tileSelector.ifTileSelected)
            {
                GameUtils.ClearHighlightedTiles(optionalTiles);
            }

            var selectedPiecePosition = gameManager.selectedPiece.GetComponent<Piece>().GetPosition();
            optionalTiles = moveOptionsProvider.GetOptionalTiles(selectedPiecePosition);

            GameUtils.HighlightTiles(optionalTiles);
            tileSelector.SetOptionalTiles(optionalTiles);
            tileSelector.SetTileSelected(true);
        }
    }
}
