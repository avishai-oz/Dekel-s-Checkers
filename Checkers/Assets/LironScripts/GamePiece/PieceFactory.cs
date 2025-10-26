using UnityEngine;

namespace DefaultNamespace
{
    public static class PieceFactory
    {
        private static PieceManager _pieceManager;
        public static void Initialize(PieceManager pieceManager)
        {
            _pieceManager = pieceManager;
        }
        public static void CreatePiece(GameObject gameObject, Vector3 coordinates, Vector2Int position)
        {
            if (_pieceManager == null)
            {
                Debug.LogError("PieceManager is not initialized.");
                return;
            }

            var pieceObject = Object.Instantiate(gameObject, coordinates, Quaternion.Euler(-90, 0, 0));
            var piece = pieceObject.GetComponent<Piece>();
            piece.SetPosition(position);
            _pieceManager.pieces.Add(piece);
        }
    }
}