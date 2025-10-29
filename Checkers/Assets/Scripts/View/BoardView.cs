using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Checkers.Domain;

namespace Checkers.View
{
    public class BoardView : MonoBehaviour, IBoardView
    {
        [SerializeField] private Transform boardRoot;

        private readonly Dictionary<Coord, Transform> _tiles = new();

        void Awake()
        {
            if (boardRoot == null)
            {
                Debug.LogError("Board root is not assigned.");
                return;
            }
            
            _tiles.Clear();
            var rx = new Regex(@"\((\d+)\s*(\b)\)");
            
            foreach (Transform child in boardRoot)
            {
                var m = rx.Match(child.name);
                if (!m.Success) continue;
                int r = int.Parse(m.Groups[1].Value);
                int c = int.Parse(m.Groups[2].Value);
                _tiles[new Coord(r, c)] = child;
            }
        }


        public void ShowPosition(BoardState board)
        {
            throw new NotImplementedException();
        }

        public void ClearHighlights()
        {
            throw new NotImplementedException();
        }
    }
}