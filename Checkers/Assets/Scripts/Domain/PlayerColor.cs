namespace Checkers.Domain
{
    public enum PlayerColor 
    {
        White = 0, 
        Black = 1 
    }

    public static class PlayerColorExt
    {
        public static PlayerColor Opponent(this PlayerColor c) =>
            c == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;
    }
}