using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public static class GameUtils
    {
        public static void HighlightTiles(List<Moves.Move> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.destentionTile.SetRendererActive(true);
            }
        }

        public static void ClearHighlightedTiles(List<Moves.Move> tiles)
        {
            foreach (var tile in tiles)
            {
                tile.destentionTile.SetRendererActive(false);
            }
        }
        public static void ChangeTileMaterialColor(List<Moves.Move> tiles, Color color)
        {
            foreach (var tile in tiles)
            {
                tile.destentionTile.SetRendererColor(color);
            }
        }
        public static List<Vector2Int> GetDiagonalPositions()
        {
            return new List<Vector2Int>
            {
                new Vector2Int(1, 1),
                new Vector2Int(1, -1),
                new Vector2Int(-1, 1),
                new Vector2Int(-1, -1)
            };
        }
        public static List<Vector2Int> GetDiagonalPositionsByPieceType(PieceType currentPieceType)
        {
            var diagonalPositions = GetDiagonalPositions();
            if (currentPieceType == PieceType.White)
            {
                diagonalPositions = diagonalPositions.GetRange(0, 2);
            }
            else if (currentPieceType == PieceType.Black)
            {
                diagonalPositions = diagonalPositions.GetRange(2, 2);
            }

            return diagonalPositions;
        }
    }
}