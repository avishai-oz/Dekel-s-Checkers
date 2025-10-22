using System;
using JetBrains.Annotations;
using UnityEngine;

namespace DefaultNamespace
{
    public class Tile : MonoBehaviour
    {
        public Piece piece;
        [SerializeField] Position _position;
        [SerializeField] Collider collision;
        [SerializeField] private GridManager _gridManager; 
        [SerializeField] private new Renderer renderer;

        public void Awake()
        {
            gameObject.name = $"({_position.x},{_position.y})";
            _gridManager.RegisterTile(this);
        }

        public Vector2Int GetPosition()
        {
            return new Vector2Int(_position.x, _position.y);
        }

        private void OnTriggerEnter(Collider other)
        {
            Piece collidedPiece = other.GetComponent<Piece>();
            if (collidedPiece != null)
            {
                piece = collidedPiece;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            Piece collidedPiece = other.GetComponent<Piece>();
            if (collidedPiece != null && piece == collidedPiece)
            {
                piece = null;
            }
        }

        public void SetRendererActive(bool isActive)
        {
             renderer.enabled = isActive;
        }
        public void SetRendererColor(Color color)
        {
            if (renderer != null)
            {
                renderer.material.color = color;
            }
        }
    }
}