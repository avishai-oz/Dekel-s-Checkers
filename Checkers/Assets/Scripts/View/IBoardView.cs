using System;
using System.Collections.Generic;
using Checkers.Domain;

namespace Checkers.View
{
    public interface IBoardView
    {
        public void ShowPosition(BoardState board);
        public void ClearHighlights();
        void HighlightTargets(IEnumerable<Coord> coords);
        
        event Action<Coord> TileClicked;
    }
}