using System.Collections.Generic;

namespace Checkers.Domain
{
    public sealed class Move
    {
        public Coord From { get; }
        public Coord To   { get; }
        public IReadOnlyList<Coord> Captured { get; }

        public bool IsCapture => Captured.Count > 0;

        public Move(Coord from, Coord to, IReadOnlyList<Coord> captured)
        {
            From = from;
            To   = to;
            Captured = captured ?? System.Array.Empty<Coord>();
        }
        
        public static Move Simple(Coord from, Coord to) =>
            new Move(from, to, System.Array.Empty<Coord>());
        

    }
}