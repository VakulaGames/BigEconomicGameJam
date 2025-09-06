using System;
using CORE;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class InputHandler : AbstractMonoService, IInitializable, IUpdatable
    {
        [Header("Input Settings")]
        public float _mouseSensitivity = 500;
        public KeyCode _runKey = KeyCode.LeftShift;
        public KeyCode _jumpKey = KeyCode.Space;
        public KeyCode _pauseKey = KeyCode.Escape;
        public KeyCode _clickAction = KeyCode.Mouse0;

        public event Action<PlayerInputData> OnInputUpdated;
        public event Action OnPause;
        public event Action OnClick;

        private PlayerInputData _currentInput;
        private bool _isInputEnabled = false;
        private bool _isCursorLocked = true;

        public override Type RegisterType => typeof(InputHandler);

        public void Init()
        {
            _currentInput = new PlayerInputData();
        }

        public void OnUpdate()
        {
            if (Input.GetKeyDown(_pauseKey))
            {
                OnPause?.Invoke();
            }
            
            if (!_isInputEnabled) return;

            UpdateInputData();
            OnInputUpdated?.Invoke(_currentInput);
            
            if (Input.GetKeyDown(_clickAction))
            {
                OnClick?.Invoke();
            }
        }

        public void SetEnable(bool enable)
        {
            _isInputEnabled = enable;

            if (enable)
            {
                LockCursor();
            }
            else
            {
                UnlockCursor();
            }
        }
        
        private void UpdateInputData()
        {
            _currentInput.Reset();

            _currentInput.Movement = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );

            if (_isCursorLocked)
            {
                _currentInput.MouseLook = new Vector2(
                    Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime,
                    Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime
                );
            }

            _currentInput.IsRunning = Input.GetKey(_runKey);
            _currentInput.JumpPressed = Input.GetKeyDown(_jumpKey);
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _isCursorLocked = true;
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _isCursorLocked = false;
        }
    }
}