namespace Checkers.Domain
{
    public sealed class Piece
    {
        public PlayerColor Owner { get; }
        public PieceKind   Kind  { get; private set; } 

        public Piece(PlayerColor owner, PieceKind kind = PieceKind.Single)
        {
            Owner = owner;
            Kind  = kind;
        }

        public void Crown()
        {
            if (Kind == PieceKind.Single)
                Kind = PieceKind.Queen;
        }
    }
}