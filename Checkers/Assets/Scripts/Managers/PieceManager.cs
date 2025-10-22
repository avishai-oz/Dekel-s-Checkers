using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Move;
using UnityEngine;

namespace DefaultNamespace
{
    public class PieceManager : MonoBehaviour, IPieceManager
    {
        [SerializeField] private PieceType _currentPieceType;
        [SerializeField] private GameManager GameManager;
        public List<Piece> pieces;
        public event Action<Piece> OnPieceEaten;


        public PieceType currentPieceType
        {
            get => _currentPieceType;
            set => _currentPieceType = value;
        }

        public int blackPiecesCount = 12;
        public int whitePiecesCount = 12;

        public void ChangeCurrentPieceType()
        {
            _currentPieceType =
                _currentPieceType == PieceType.Black ? PieceType.White : PieceType.Black;
            //cameraMovement.MoveCameraToSide(pieceManager.currentPieceType);
        }

        public void RemovePieceFromManager([CanBeNull] Piece piece)
        {
            switch (piece?.type)
            {
                case PieceType.Black:
                    blackPiecesCount--;
                    break;
                case PieceType.White:
                    whitePiecesCount--;
                    break;
            }
            pieces.Remove(piece);
        }

        public void EatPiece(Piece piece)
        {
            OnPieceEaten?.Invoke(piece);
            Destroy(piece.gameObject);
            RemovePieceFromManager(piece);
        }

        public Piece GetPieceAtPosition(Vector2Int position)
        {
            foreach (var piece in pieces)
            {
                var piecePosition = piece.GetPosition();
                if (piecePosition == position)
                {
                    return piece;
                }
            }
            return null;
        }
    }
}