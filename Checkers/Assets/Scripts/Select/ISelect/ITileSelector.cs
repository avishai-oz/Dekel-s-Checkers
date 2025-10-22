using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public interface ITileSelector
{
    void SelectTile(Tile tile);
    event Action<Tile> OnTileSelected;
}