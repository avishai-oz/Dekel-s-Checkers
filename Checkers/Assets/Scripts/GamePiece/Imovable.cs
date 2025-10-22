// File: Assets/Scripts/Interfaces/IMovable.cs
using UnityEngine;

namespace DefaultNamespace
{
    public interface IMovable
    {
        void MoveTo(Vector3 position);
        void SetPosition(Vector2Int position);
        Vector2Int GetPosition();
        PieceType GetPieceType();

    }
}