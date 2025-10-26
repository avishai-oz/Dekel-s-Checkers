using System.Collections.Generic;
using DefaultNamespace;
using Moves;
using Select;
using UnityEngine;

namespace Managers
{
    public class MoveManager : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private SelectionManager selectionManager;
        [SerializeField] private PieceManager pieceManager;
        [SerializeField] private GridManager gridManager;
        private MoveOptionsProvider _moveOptionsProvider;
        private Vector2Int _previousPosition;
        private Moves.Move move;
        
        public void Awake()
        {
            _moveOptionsProvider = new MoveOptionsProvider(gridManager, gameManager, pieceManager);
        }


        private void OnEnable()
        {
            if (selectionManager.tileSelector == null)
            {
                Debug.LogError("TileSelector is not initialized.");
                return;
            }

            selectionManager.tileSelector.OnTileSelected += MovePieceToTile;
            gameManager.OnPieceEaten += pieceManager.EatPiece;
        }

        private void OnDisable()
        {
            selectionManager.tileSelector.OnTileSelected -= MovePieceToTile;
            gameManager.OnPieceEaten -= pieceManager.EatPiece;
        }

        public void MovePieceToTile(Tile tile)
        {
            var selectedPiece = gameManager.selectedPiece.GetComponent<IMovable>();
            var eatenPiece = _moveOptionsProvider.TryGetEatenPiece(selectedPiece.GetPosition(), tile);
            Vector3 targetPosition = tile.transform.position;
            selectedPiece.MoveTo(targetPosition);
            _previousPosition = selectedPiece.GetPosition();
            SetPiecePositionToNewPosition(selectedPiece);

            gameManager.selectedPiece = null;
            gameManager.selectedTile = null;

            var canEatAgain = _moveOptionsProvider.CanEatAgain(selectedPiece.GetPosition());
            if (eatenPiece)
            {
                gameManager.TriggerPieceEatenEvent(eatenPiece, canEatAgain);
            }
            else
            {
                gameManager.TriggerPieceMovedEvent();
            }
        }

        private void SetPiecePositionToNewPosition(IMovable piece)
        {
            var tilePosition = gameManager.selectedTile.GetComponent<Tile>().GetPosition();
            piece.SetPosition(tilePosition);
        }
        
        
    }
}