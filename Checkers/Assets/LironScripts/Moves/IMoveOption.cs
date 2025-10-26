using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Moves
{
    public interface IMoveOption
    {
        List<Move> GetMoveTiles(Vector2Int position);
    }
}