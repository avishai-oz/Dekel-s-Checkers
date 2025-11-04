using System;
using Checkers.Domain;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Checkers.View
{
    public class TileClick: MonoBehaviour , IPointerClickHandler
    {
        public Coord Coord{get; set;}
        public event Action<Coord> Clicked;

        void Start() { Debug.Log("Tile ready " + Coord); }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(Coord);
            Debug.Log("Tile clicked " + Coord);
        }
    }
}