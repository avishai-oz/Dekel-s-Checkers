using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GridManager : MonoBehaviour , IGridManager
{
    [SerializeField] private GameObject whitePiecePrefab;
    [SerializeField] private GameObject blackPiecePrefab;
    public Tile[,] tileObjects;

    void Start()
    {
        PlaceWhitePieces();
        PlaceBlackPieces();
    }

    public void RegisterTile(Tile tile)
    {
        tileObjects ??= new Tile[8, 8];
        Vector2Int position = tile.GetPosition();
        tileObjects[position.x, position.y] = tile;
    }

    public Tile GetTileAtPosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < tileObjects.GetLength(0) && position.y >= 0 &&
            position.y < tileObjects.GetLength(1))
        {
            return tileObjects[position.x, position.y];
        }

        return null;
    }

    private void PlaceWhitePieces()
    {
        for (int i = 0; i < 3; i++)
        {
            int startColumn = (i == 1) ? 1 : 0;
            for (int j = startColumn; j < 8; j += 2)
            {
                if (tileObjects[i, j] != null)
                {
                    Vector3 globalCoordinates = tileObjects[i, j].transform.position;
                    Vector2Int position = new Vector2Int(i, j);
                    PieceFactory.CreatePiece(whitePiecePrefab, globalCoordinates, position);
                }
            }
        }
    }

    private void PlaceBlackPieces()
    {
        for (int i = 5; i < 8; i++)
        {
            int startColumn = (i == 6) ? 0 : 1;
            for (int j = startColumn; j < 8; j += 2)
            {
                if (tileObjects[i, j] != null)
                {
                    Vector3 globalCoordinates = tileObjects[i, j].transform.position;
                    Vector2Int position = new Vector2Int(i, j);
                    PieceFactory.CreatePiece(blackPiecePrefab, globalCoordinates, position);
                }
            }
        }
    }
}