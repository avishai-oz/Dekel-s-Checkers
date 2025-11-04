using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Checkers.Domain;
using Checkers.Presenter;

namespace Checkers.View
{
    public class BoardView : MonoBehaviour, IBoardView
    {
        [Header("board")]
        [SerializeField] private Transform boardRoot;
        private readonly Dictionary<Coord, Transform> _tiles = new();
        
        [Header("highlighting")]
        [SerializeField] private Material highlightMat;
        private readonly Dictionary<Coord, Renderer> _highlightRends = new();
        private readonly HashSet<Coord> _highlighted = new();
        
        [Header("pices")]
        [SerializeField] private Transform piecesRoot; // הורה לכל החיילים בסצנה
        [SerializeField] private GameObject whiteSinglePrefab;
        [SerializeField] private GameObject blackSinglePrefab;
        [SerializeField] private GameObject whiteQueenPrefab;
        [SerializeField] private GameObject blackQueenPrefab;
        [SerializeField] private float pieceYOffset = 0.3f; // כמה להרים את החייל מעל האריח
        private readonly Dictionary<Coord, GameObject> _pieces = new();
        public event Action<Coord> TileClicked;
        
        void Awake()
        {
            if (boardRoot == null)
            {
                Debug.LogError("Board root is not assigned.");
                return;
            }
            
            _tiles.Clear();

            foreach (Transform child in boardRoot)
            {
                if (!TryGetCoordFromName(child.name, out int r, out int c))
                    continue;

                var coord = new Coord(r, c);
                _tiles[coord] = child;
                
                var click = child.GetComponent<TileClick>();
                if (click == null) click = child.gameObject.AddComponent<TileClick>();
                click.Coord = coord;
                click.Clicked -= HandleTileClicked;
                click.Clicked += HandleTileClicked;
                
                var h = child.Find("Highlight");
                if (h != null)
                {
                    var hr = h.GetComponent<Renderer>();
                    if (hr != null)
                    {
                        if (highlightMat != null) hr.sharedMaterial = highlightMat; 
                        hr.enabled = false;                                         
                        _highlightRends[coord] = hr;
                    }
                }
            }

            Debug.Log($"BoardViewMB: indexed {_tiles.Count} tiles");
        }

        private void HandleTileClicked(Coord c)
        {
            Debug.Log($"BoardView → TileClicked({c})");
            TileClicked?.Invoke(c);
        }

        private static bool TryGetCoordFromName(string name, out int r, out int c)
        {
            r = c = 0;
            if (string.IsNullOrEmpty(name) || name[0] != '(' || name[^1] != ')') return false;
            int comma = name.IndexOf(',');
            if (comma < 0) return false;
            return int.TryParse(name.Substring(1, comma-1).Trim(), out r)
                   && int.TryParse(name.Substring(comma+1, name.Length-comma-2).Trim(), out c);
        }

        public void HighlightTargets(IEnumerable<Coord> coords)
        {
            ClearHighlights();
            int n = 0;
            foreach (var c in coords)
                if (_highlightRends.TryGetValue(c, out var r))
                {
                    r.enabled = true;
                    _highlighted.Add(c);
                    n++;
                }
            Debug.Log($"BoardView.HighlightTargets: turned on {n}");

        }

        public void ClearHighlights()
        {
            foreach (var c in _highlighted)
                if (_highlightRends.TryGetValue(c, out var r))
                    r.enabled = false;
            _highlighted.Clear();
        }
        public void ShowPosition(BoardState board)
        {
            Debug.Log("BoardView.ShowPosition CALLED");
            // 1) נקה ציורים קודמים
            _pieces.Clear();
            if (piecesRoot != null)
            {
                for (int i = piecesRoot.childCount - 1; i >= 0; i--)
                {
                    var child = piecesRoot.GetChild(i);
                }
            }
            int placed = 0;

            // 2) עבור על כל התאים וצייר
            for (int r = 0; r < BoardState.Size; r++)
            for (int c = 0; c < BoardState.Size; c++)
            {
                var p = board[r, c];
                if (p == null) continue;

                var coord = new Coord(r, c);

                // מיקום המשבצת בעולם
                if (!_tiles.TryGetValue(coord, out var tileTf)) continue;

                var prefab = GetPiecePrefab(p);
                if (prefab == null) continue;

                var go = Instantiate(prefab, piecesRoot);
                var pos = tileTf.position;
                go.transform.position = new Vector3(pos.x, pos.y + pieceYOffset, pos.z);

                _pieces[coord] = go;
                
                placed++;
            }
            Debug.Log($"BoardView.ShowPosition DONE, placed={placed}");

        }

        private GameObject GetPiecePrefab(Piece p)
        {
            if (p.Owner == PlayerColor.White)
                return p.Kind == PieceKind.Single ? whiteSinglePrefab : whiteQueenPrefab;
            else
                return p.Kind == PieceKind.Single ? blackSinglePrefab : blackQueenPrefab;
        }

    }
}