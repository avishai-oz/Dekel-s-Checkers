using System.Collections;
using DefaultNamespace;
using UnityEngine;

namespace Moves
{
    public class CameraMovement : MonoBehaviour
    {
        private Vector3 blackSidePosition = new Vector3(8, 7, 0);
        private Vector3 whiteSidePosition = new Vector3(-6, 7, 0);
        private Quaternion blackSideRotation = Quaternion.Euler(55, -90, 0);
        private Quaternion whiteSideRotation = Quaternion.Euler(55, 90, 0);
        [SerializeField] private float transitionDuration = 1f;

        public void MoveCameraToSide(PieceType pieceType)
        {
            Vector3 targetPosition = pieceType == PieceType.Black ? blackSidePosition : whiteSidePosition;
            Quaternion targetRotation = pieceType == PieceType.Black ? blackSideRotation : whiteSideRotation;
            StartCoroutine(SmoothTransition(targetPosition, targetRotation));
        }

        private IEnumerator SmoothTransition(Vector3 targetPosition, Quaternion targetRotation)
        {
            Vector3 startPosition = transform.position;
            Quaternion startRotation = transform.rotation;
            float elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / transitionDuration);
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / transitionDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPosition;
            transform.rotation = targetRotation;
        }
    }
}