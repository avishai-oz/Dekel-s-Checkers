using UnityEngine;

namespace Checkers.infrastucture
{
    public interface ITweenService
    {
        void Move(Transform target, Vector3 toWorldPos, float duration, System.Action onComplete = null);
    }
}