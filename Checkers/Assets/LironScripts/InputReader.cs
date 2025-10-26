using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    [CreateAssetMenu(menuName = "InputReader")]
    public class InputReader : ScriptableObject, InputMap.IPlayerActions
    {
        private InputMap _inputMap;

        public void OnEnable()
        {
            if (_inputMap == null)
            {
                _inputMap = new InputMap();
                _inputMap.Player.SetCallbacks(this);
            }

            _inputMap.Enable();
        }

        public void OnDisable()
        {
            _inputMap.Disable();
        }

        public event Action<Vector2> OnClickEvent;
        public event Action OnPauseEvent;

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Vector3 mousePosition = Mouse.current.position.ReadValue();
                OnClickEvent?.Invoke(mousePosition);
            }
        }


        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                OnPauseEvent?.Invoke();
            }
        }
    }
}