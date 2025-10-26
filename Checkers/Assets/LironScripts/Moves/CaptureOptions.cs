using System.Collections.Generic;
using DefaultNamespace;
using Move;
using UnityEngine;

namespace Moves
{
    public class CaptureOption
    {
        private readonly IGridManager _gridManager;
        private readonly IPieceManager _pieceManager;
        private readonly IGameManager _gameManager;


        public CaptureOption(IGridManager gridManager, IPieceManager pieceManager, IGameManager gameManager)
        {
            _gridManager = gridManager;
            _pieceManager = pieceManager;
            _gameManager = gameManager;
        }

        public List<Move> GetCaptureMoves(Vector2Int position)
        {
            var captureMoves = new List<Move>();
            var diagonalPositions = GameUtils.GetDiagonalPositionsByPieceType(_pieceManager.currentPieceType);

            foreach (var direction in diagonalPositions)
            {
                var firstPosition = position + direction;
                var firstTile = _gridManager.GetTileAtPosition(firstPosition);

                if (CanCaptureTile(firstTile, _pieceManager.currentPieceType))
                {
                    var secondTile = GetTileAfterPieceCaptured(firstPosition, direction);
                    var eatanPiece = GetCapturedPiece(firstPosition);
                    if (secondTile != null && secondTile.piece == null)
                    {
                        var move = new Move(secondTile, eatanPiece, MoveType.Capture);
                        captureMoves.Add(move);
                    }
                }
            }

            return captureMoves;
        }


        private bool CanCaptureTile(Tile firstTile, PieceType selectedPieceType)
        {
            return firstTile != null && firstTile.piece != null && firstTile.piece.IsOpposite(selectedPieceType);
        }

        private Tile GetTileAfterPieceCaptured(Vector2Int firstPosition, Vector2Int direction)
        {
            var secondPosition = firstPosition + direction;
            return _gridManager.GetTileAtPosition(secondPosition);
        }

        private Piece GetCapturedPiece(Vector2Int capturedPosition)
        {
            var capturedPiece = _pieceManager.GetPieceAtPosition(capturedPosition);
            if (capturedPiece == null)
            {
                return null;
            }

            return capturedPiece;
        }
    }
}