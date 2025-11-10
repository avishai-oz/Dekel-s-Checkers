using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Checkers.Domain;
using Checkers.Presenter;
using Checkers.infrastucture;

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
        [SerializeField] private GameObject whiteSinglePrefab;
        [SerializeField] private GameObject blackSinglePrefab;
        [SerializeField] private GameObject whiteQueenPrefab;
        [SerializeField] private GameObject blackQueenPrefab;
        [SerializeField] private float pieceYOffset = 0.3f; 
       
        [Header("animation")]
        [SerializeField] private CoroutineTweenService tweenService;
        [SerializeField] private float moveDuration = 0.25f;
        
        private readonly Dictionary<Coord, GameObject> _pieces = new(); 
        private readonly Dictionary<Coord, Transform> _slots = new(); 
        private BoardState _lastBoard;
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
                
                var slot = child.Find("PieceSlot");
                if (slot == null) {
                    var go = new GameObject("PieceSlot");
                    go.transform.SetParent(child, false);
                    slot = go.transform;
                }
                _slots[coord] = slot;

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
            if (_lastBoard == null)
            {
                BuildAll(board);              
                _lastBoard = board.Clone();
                return;
            }
        
            for (int r = 0; r < BoardState.Size; r++)
            for (int c = 0; c < BoardState.Size; c++)
            {
                var coord = new Coord(r, c);
                var oldP  = _lastBoard[r, c];
                var newP  = board[r, c];
        
                if (SamePiece(oldP, newP)) continue; // pice unchanged
        
                if (oldP != null && newP == null) // piece removed
                {
                    if (_pieces.TryGetValue(coord, out var go)) {
                        Destroy(go);
                        _pieces.Remove(coord);
                    }
                    continue;
                }
        
                if (oldP == null && newP != null)
                {
                    if (_pieces.TryGetValue(coord, out var existingAtTo) && existingAtTo != null)
                    {
                        var tag = existingAtTo.GetComponent<PieceViewTag>();
                        bool sameLook = tag != null && tag.Owner == newP.Owner && tag.Kind == newP.Kind;

                        if (sameLook)
                        {
                            AttachToSlot(existingAtTo, coord);
                            continue;
                        }
                        else
                        {
                            Destroy(existingAtTo);
                            _pieces.Remove(coord);
                        }
                    }

                    var prefab = GetPiecePrefab(newP);
                    if (prefab == null || !_slots.TryGetValue(coord, out var slot)) continue;

                    var go = Instantiate(prefab, slot);
                    go.transform.localPosition = new Vector3(0, pieceYOffset, 0);
                    SetTag(go, newP);
                    _pieces[coord] = go;
                    continue;
                }
        
                if (oldP.Owner != newP.Owner || oldP.Kind != newP.Kind)
                {
                    if (_pieces.TryGetValue(coord, out var oldGo)) Destroy(oldGo);
                    var prefab = GetPiecePrefab(newP);
                    if (prefab == null || !_slots.TryGetValue(coord, out var slot)) continue;
                    var go = Instantiate(prefab, slot);
                    SetTag(go, newP);
                    go.transform.localPosition = new Vector3(0, pieceYOffset, 0);
                    _pieces[coord] = go;
                }
            }
        
            _lastBoard = board.Clone();
        }
        bool SamePiece(Piece a, Piece b)
        {
            if (a == null && b == null) return true;
            if (a == null ||  b == null) return false;
            return a.Owner == b.Owner && a.Kind == b.Kind;
        }
        void BuildAll(BoardState board)
        {
            _pieces.Clear();
            for (int r = 0; r < BoardState.Size; r++)
            for (int c = 0; c < BoardState.Size; c++)
            {
                var p = board[r, c];
                if (p == null) continue;
                var coord = new Coord(r, c);
                if (!_slots.TryGetValue(coord, out var slot)) continue;
                var prefab = GetPiecePrefab(p);
                if (prefab == null) continue;
                var go = Instantiate(prefab, slot);
                SetTag(go, p);
                go.transform.localPosition = new Vector3(0, pieceYOffset, 0);
                _pieces[coord] = go;
            }
        }
        private GameObject GetPiecePrefab(Piece p)
        {
            if (p.Owner == PlayerColor.White)
                return p.Kind == PieceKind.Single ? whiteSinglePrefab : whiteQueenPrefab;
            else
                return p.Kind == PieceKind.Single ? blackSinglePrefab : blackQueenPrefab;
        }
        private void AttachToSlot(GameObject go, Coord coord)
        {
            if (!_slots.TryGetValue(coord, out var slot)) return;
            go.transform.SetParent(slot, true);
            go.transform.localPosition = new Vector3(0, pieceYOffset, 0);
        }

        private void SetTag(GameObject go, Piece p)
        {
            var tag = go.GetComponent<PieceViewTag>();
            if (tag == null) tag = go.AddComponent<PieceViewTag>();
            tag.Owner = p.Owner;
            tag.Kind  = p.Kind;
        }
        private List<Coord> ComputeLandings(Coord from, IReadOnlyList<Coord> captured)
        {
            var landings = new List<Coord>(Mathf.Max(1, captured.Count));
            var cur = from;
            foreach (var mid in captured)
            {
                int dr = mid.Row - cur.Row;
                int dc = mid.Col - cur.Col;
                var landing = new Coord(cur.Row + 2*dr, cur.Col + 2*dc);
                landings.Add(landing);
                cur = landing;
            }
            return landings; 
        }
        
        public void AnimateMove(Move move, System.Action onComplete)
        {
            if (!_pieces.TryGetValue(move.From, out var go))
            {
                Debug.LogWarning($"AnimateMove: no piece at {move.From}");
                onComplete?.Invoke();
                return;
            }

            var landings = ComputeLandings(move.From, move.Captured);
            if (landings.Count == 0) landings.Add(move.To); 

            var worldPoints = new List<Vector3>(landings.Count);
            foreach (var coord in landings)
            {
                if (!_slots.TryGetValue(coord, out var slot))
                {
                    Debug.LogWarning($"AnimateMove: missing slot at {coord}");
                    continue;
                }
                worldPoints.Add(slot.TransformPoint(new Vector3(0, pieceYOffset, 0)));
            }

            tweenService.MoveSequenceBySpeed(go.transform, worldPoints, moveDuration, onHop: i =>
                {
                    if (i < move.Captured.Count && _pieces.TryGetValue(move.Captured[i], out var dead))
                    {
                        Destroy(dead);
                        _pieces.Remove(move.Captured[i]);
                    }
                },
                onComplete: () =>
                {
                    _pieces.Remove(move.From);
                    _pieces[move.To] = go;

                    if (_slots.TryGetValue(move.To, out var toSlot))
                    {
                        go.transform.SetParent(toSlot, true);
                        go.transform.localPosition = new Vector3(0, pieceYOffset, 0);
                    }

                    onComplete?.Invoke();
                });
        }

        

    }
}