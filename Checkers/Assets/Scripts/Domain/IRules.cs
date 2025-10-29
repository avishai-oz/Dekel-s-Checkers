using System.Collections.Generic;

namespace Checkers.Domain
{
    public interface IRules
    {
        IReadOnlyList<Move> LegalMoves(BoardState board, PlayerColor side);
        
        bool IsGameOver(BoardState board, PlayerColor side,out PlayerColor? winner);
        
        bool ShouldCrown(Piece piece, Coord to);
        
    }
}