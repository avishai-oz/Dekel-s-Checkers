using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;

namespace Select
{
    public class HoverPieceSelector : MonoBehaviour, IPieceSelector
    {
        [SerializeField] private GameManager gameManager;
        private bool _unitSelected = false;
        private Vector3 _originalPosition;

        public async UniTask SelectPiece(Piece piece)
        {
            if (gameManager.selectedPiece == piece)
            {
                return;
            }

            DeselectPiece();
            await SelectAndMovePiece(piece);
        }

        private void DeselectPiece()
        {
            if (!_unitSelected || gameManager.selectedPiece == null) return;
            LeanTween.move(gameManager.selectedPiece.gameObject, _originalPosition, 0.2f);
            gameManager.selectedPiece = null;
            _unitSelected = false;
        }

        private UniTask SelectAndMovePiece(Piece piece)
        {
            var tcs = new UniTaskCompletionSource();
            _originalPosition = piece.transform.position;
            gameManager.selectedPiece = piece;
            LeanTween.move(piece.gameObject, _originalPosition + new Vector3(0, 0.2f, 0), 0.2f).setOnComplete(() =>
            {
                _unitSelected = true;
                tcs.TrySetResult();
            });
            return tcs.Task;
        }
    }
}