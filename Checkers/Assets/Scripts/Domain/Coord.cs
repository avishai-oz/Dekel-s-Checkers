using System;

namespace Checkers.Domain
{
    public readonly struct Coord : IEquatable<Coord>
    {
        public int Row { get; }
        public int Col { get; }

        public Coord(int row, int col)
        {
            Row = row; Col = col;
        }

        public bool IsDarkSquare => (Row + Col) % 2 == 1;

        public bool Equals(Coord other) => Row == other.Row && Col == other.Col;
        public override bool Equals(object obj) => obj is Coord c && Equals(c);
        public override int GetHashCode() => (Row * 397) ^ Col;
        public override string ToString() => $"({Row},{Col})";
    }
}