using System;
using System.Collections;
using UnityEngine;

namespace Checkers.infrastucture
{
    public class CoroutineTweenService : MonoBehaviour, ITweenService
    {
        [SerializeField] private float defaultDuration = 0.25f;
        
        public void Move(Transform target, Vector3 toWorldPos, float duration, Action onComplete = null)
        {
            StartCoroutine(MoveCoroutine(target, toWorldPos, duration, onComplete));
        }
        
        private IEnumerator MoveCoroutine(Transform target, Vector3 toWorldPos, float duration, Action onComplete)
        {
            if (duration <= 0f)
                duration = defaultDuration;

            Vector3 fromWorldPos = target.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);
                target.position = Vector3.Lerp(fromWorldPos, toWorldPos, t);
                yield return null;
            }

            target.position = toWorldPos;
            onComplete?.Invoke();
        }
    }
}