using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Player
{
    [CreateAssetMenu(fileName = "PlayerInput", menuName = "SO/PlayerInput", order = 0)]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public event Action OnAttackPressed;
        public event Action OnJumpPressed;
        public event Action OnSlidePressed;
        
        public Vector2 MovementKey { get; private set; }

        public bool CanInput { get; private set; }
        
        private Controls _controls;

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
            CanInput = true;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MovementKey = context.ReadValue<Vector2>();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnAttackPressed?.Invoke();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnJumpPressed?.Invoke();
        }

        public void OnSlide(InputAction.CallbackContext context)
        {
            if(context.performed)
                OnSlidePressed?.Invoke();
        }

        public void BlockInputChange(bool value)
        {
            CanInput = value;
        }
    }
}