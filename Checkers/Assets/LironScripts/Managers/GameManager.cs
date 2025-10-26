using System;
using JetBrains.Annotations;
using Moves;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        [SerializeField] private CameraMovement cameraMovement;
        [SerializeField] private PieceManager pieceManager;
        [SerializeField] private GameStateManager gameStateManager;
        public Piece selectedPiece;
        public Tile selectedTile;
        public event Action<Piece> OnPieceEaten;
        public event Action OnPieceMoved;

        private void Awake()
        {
            PieceFactory.Initialize(pieceManager);
        }
        

        public void TriggerPieceEatenEvent(Piece piece, bool canEatAgain)
        {
            OnPieceEaten?.Invoke(piece);
            gameStateManager.CheckGameOver(pieceManager.blackPiecesCount, pieceManager.whitePiecesCount, pieceManager.currentPieceType);
            if (!canEatAgain)
            {
                pieceManager.ChangeCurrentPieceType();
            }
        }

        public void TriggerPieceMovedEvent()
        {
            OnPieceMoved?.Invoke();
            pieceManager.ChangeCurrentPieceType();
        }
    }
}