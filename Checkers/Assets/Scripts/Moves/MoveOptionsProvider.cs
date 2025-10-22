using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using Move;
using UnityEngine;

namespace Moves
{
    public class MoveOptionsProvider
    {
        private readonly IMoveOption _moveOption;
        public readonly CaptureOption _captureOption;
        private readonly IPieceManager _pieceManager;


        public MoveOptionsProvider(IGridManager gridManager, IGameManager gameManager, IPieceManager pieceManager)
        {
            _moveOption = new MoveOption(gridManager, pieceManager, gameManager);
            _captureOption = new CaptureOption(gridManager, pieceManager, gameManager);
            _pieceManager = pieceManager;
        }

        public List<Move> GetOptionalTiles(Vector2Int position)
        {
            var captureTiles = _captureOption.GetCaptureMoves(position);
            if (captureTiles.Count > 0)
            {
                return captureTiles;
            }

            return _moveOption.GetMoveTiles(position);
        }

        public Piece TryGetEatenPiece(Vector2Int position, Tile tile)
        {
            var captureTiles = _captureOption.GetCaptureMoves(position);
            if (captureTiles.Count > 0)
            {
                foreach (var capture in captureTiles)
                {
                    if (capture.destentionTile == tile)
                    {
                        return capture.EatenPiece;
                    }
                }
            }

            return null;
        }

        public bool CanEatAgain(Vector2Int position)
        {
            var captureTiles = _captureOption.GetCaptureMoves(position);
            foreach (var capture in captureTiles)
            {
                if (TryGetEatenPiece(position, capture.destentionTile) != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}