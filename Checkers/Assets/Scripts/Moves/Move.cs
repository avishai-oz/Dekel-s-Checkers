using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;

namespace Moves
{
    public class Move 
    {
        public Tile destentionTile { get; set; }
        public MoveType moveType { get; set; }
        [CanBeNull] public Piece EatenPiece { get; set; }
        
        
        public Move(Tile destentionTile, [CanBeNull] Piece eatenPiece,MoveType moveType)
        {
            this.destentionTile = destentionTile;
            EatenPiece = eatenPiece;
            this.moveType = moveType;
        }
        
    }
    public enum MoveType
    {
        Move,
        Capture
    }
}