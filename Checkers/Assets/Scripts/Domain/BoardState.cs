using System;

namespace Checkers.Domain
{
    public sealed class BoardState
    {
        public const int Size = 8;

        private readonly Piece[,] _grid = new Piece[Size, Size];

        // מגדיר this לגישה נוחה ללוח באמצעות שורות ועמודות או באמצעות אובייקט Coord
        public Piece this[int r, int c] => _grid[r, c];
        public Piece this[Coord c]      => _grid[c.Row, c.Col];

        public bool InBounds(Coord c) =>
            c.Row >= 0 && c.Row < Size && c.Col >= 0 && c.Col < Size;

        public void Place(Coord c, Piece p)
        {
            if (!InBounds(c)) 
                throw new ArgumentOutOfRangeException(nameof(c));
            _grid[c.Row, c.Col] = p;
        }

        public Piece Remove(Coord c)
        {
            if (!InBounds(c)) 
                throw new ArgumentOutOfRangeException(nameof(c));
            var old = _grid[c.Row, c.Col];
            _grid[c.Row, c.Col] = null;
            return old;
        }

        public void MovePiece(Coord from, Coord to)
        {
            if (!InBounds(from) || !InBounds(to))
                throw new ArgumentOutOfRangeException();

            var p = _grid[from.Row, from.Col];
            _grid[from.Row, from.Col] = null;
            _grid[to.Row, to.Col] = p;
        }

        public BoardState Clone()
        {
            var copy = new BoardState();
            for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
            {
                var p = _grid[r, c];
                copy._grid[r, c] = p is null ? null : new Piece(p.Owner, p.Kind);
            }
            return copy;
        }
    }
}