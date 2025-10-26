using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Select
{
    public class TileSelector : MonoBehaviour, ITileSelector
    {
        [SerializeField] private GameManager gameManager;
        private List<Moves.Move> optionalTiles = new List<Moves.Move>();
        public event Action<Tile> OnTileSelected;
        public bool ifTileSelected = false;

        public void SelectTile(Tile tile)
        {
            if (gameManager.selectedPiece == null || !optionalTiles.Exists(move => move.destentionTile == tile)) return;

            if (ifTileSelected)
            {
                GameUtils.ClearHighlightedTiles(optionalTiles);
            }
            gameManager.selectedTile = tile;
            OnTileSelected?.Invoke(tile);
        }

        public void SetOptionalTiles(List<Moves.Move> tiles)
        {
            optionalTiles = tiles;
        }

        public void SetTileSelected(bool selected)
        {
            ifTileSelected = selected;
        }
    }
}
