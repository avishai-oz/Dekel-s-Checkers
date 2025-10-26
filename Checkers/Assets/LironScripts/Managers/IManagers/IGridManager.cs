using UnityEngine;

namespace DefaultNamespace
{
    public interface IGridManager
    {
        Tile GetTileAtPosition(Vector2Int position);
    }
}