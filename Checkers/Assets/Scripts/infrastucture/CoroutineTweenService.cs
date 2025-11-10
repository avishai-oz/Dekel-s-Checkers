using System;
using System.Collections;
using System.Collections.Generic;
using Checkers.Domain;
using Checkers.View;
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

        public void MoveSequenceBySpeed(Transform target, IList<Vector3> waypoints, float unitsPerSecond, Action<int> onHop = null, Action onComplete = null)
        {
            if (target == null || waypoints == null || waypoints.Count == 0) { onComplete?.Invoke(); return; }
            StartCoroutine(MoveSequenceBySpeedRoutine(target, waypoints, Mathf.Max(2f, unitsPerSecond), onHop, onComplete));
        }

        private IEnumerator MoveSequenceBySpeedRoutine(Transform target, IList<Vector3> waypoints, float speed, Action<int> onHop, Action onComplete)
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                Vector3 from = target.position;
                Vector3 to   = waypoints[i];
                float dist   = Vector3.Distance(from, to);
                float dur    = dist / speed;

                float elapsed = 0f;
                while (elapsed < dur)
                {
                    if (target == null) { onComplete?.Invoke(); yield break; }
                    elapsed += Time.deltaTime;
                    float t = Mathf.Clamp01(elapsed / dur);
                    float k = Mathf.SmoothStep(0f, 1f, t);
                    target.position = Vector3.LerpUnclamped(from, to, k);
                    yield return null;
                }
                target.position = to;
                onHop?.Invoke(i);
                yield return null;
            }
            onComplete?.Invoke();
        }
        
    }

    
}