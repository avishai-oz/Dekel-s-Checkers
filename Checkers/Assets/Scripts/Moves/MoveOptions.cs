using System.Collections.Generic;
using DefaultNamespace;
using Move;
using UnityEngine;

namespace Moves
{
    public class MoveOption : IMoveOption
    {
        private readonly IGridManager _gridManager;
        private readonly IPieceManager _pieceManager;
        private readonly IGameManager _gameManager;

        public MoveOption(IGridManager gridManager, IPieceManager pieceManager, IGameManager gameManager)
        {
            _gridManager = gridManager;
            _pieceManager = pieceManager;
            _gameManager = gameManager;
            
        }
       
        public List<Move> GetMoveTiles(Vector2Int position)
        {
            var optionalMoves = new List<Move>();

            var diagonalPositions = GameUtils.GetDiagonalPositionsByPieceType(_pieceManager.currentPieceType);
            foreach (var direction in diagonalPositions)
            {
                var newPosition = position + direction;
                var tile = _gridManager.GetTileAtPosition(newPosition);
                if (tile != null && tile.piece == null)
                {
                    var move = new Move(tile, null, MoveType.Move);
                    optionalMoves.Add(move);
                }
            }
            return optionalMoves;
        }

        
    }
}