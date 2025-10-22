// File: Assets/Scripts/GamePiece/Piece.cs

using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace
{
    public class Piece : MonoBehaviour, IMovable
    {
        [SerializeField] public PieceType type;
        private bool isQueen = false;
        public Position _position;

        private void OnEnable()
        {
            var moveManager = FindObjectOfType<MoveManager>();
        }

        private void OnDisable()
        {
            var moveManager = FindObjectOfType<MoveManager>();
        }

        public Vector2Int GetPosition()
        {
            int x = _position.x;
            int y = _position.y;
            return new Vector2Int(x, y);
        }

        public PieceType GetPieceType()
        {
            return type;
        }

        public void SetPosition(Vector2Int position)
        {
            _position.x = position.x;
            _position.y = position.y;
        }

        public void MoveTo(Vector3 position)
        {
            var currentPos = new Vector2Int(_position.x, _position.y);
            var direction = new Vector2Int((int)position.x - currentPos.x, (int)position.y - currentPos.y);
            LeanTween.move(gameObject, position, 0.2f).setEase(LeanTweenType.easeInOutQuad);
        }

        public bool IsOpposite(PieceType otherType)
        {
            return (this.type == PieceType.Black && otherType == PieceType.White) ||
                   (this.type == PieceType.White && otherType == PieceType.Black);
        }
        
    }
}