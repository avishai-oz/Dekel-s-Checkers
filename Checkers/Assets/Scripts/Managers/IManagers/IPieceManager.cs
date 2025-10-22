using DefaultNamespace;
using UnityEngine;

namespace Move
{
    public interface IPieceManager
    {
        PieceType currentPieceType { get; set; }
        Piece GetPieceAtPosition(Vector2Int position);
    }
}