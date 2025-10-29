

using Checkers.Domain;

namespace Checkers.View
{
    public interface IBoardView
    {
        public void ShowPosition(BoardState board);
        public void ClearHighlights();
    }
}