using System;
using CORE;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class FirstPersonController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float walkSpeed = 5f;
        public float runSpeed = 8f;
        public float jumpForce = 5f;
        public float gravity = -9.81f;
        
        [Header("Mouse Look Settings")]
        public Transform PlayerCamera;
        public float maxLookAngle = 90f;
        
        [SerializeField] private CharacterController _controller;

        private InputHandler _inputHandler;
        
        private Vector3 _velocity;
        private bool _isGrounded;
        private float _xRotation = 0f;
        
        private bool _isControlEnabled = true;

        public void Init()
        {
            _inputHandler = ServiceLocator.GetService<InputHandler>();
            
            _inputHandler.OnInputUpdated += HandleInput;
        }

        public void OnUpdate()
        {
            if (!_isControlEnabled) return;

            UpdateGravity();
        }

        public void SetEnableControl(bool enable)
        {
            _isControlEnabled = enable;
            _inputHandler.SetEnable(enable);
        }

        private void HandleInput(PlayerInputData input)
        {
            if (!_isControlEnabled) return;

            HandleMouseLook(input.MouseLook);
            HandleMovement(input.Movement, input.IsRunning);
            //HandleJump(input.JumpPressed);
        }

        private void HandleMouseLook(Vector2 mouseLook)
        {
            _xRotation -= mouseLook.y;
            _xRotation = Mathf.Clamp(_xRotation, -maxLookAngle, maxLookAngle);
            
            PlayerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseLook.x);
        }

        private void HandleMovement(Vector2 movement, bool isRunning)
        {
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            
            Vector3 move = transform.right * movement.x + transform.forward * movement.y;
            
            _controller.Move(move * currentSpeed * Time.deltaTime);
        }

        private void UpdateGravity()
        {
            _isGrounded = _controller.isGrounded;
            
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            
            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        private void HandleJump(bool jumpPressed)
        {
            Debug.Log($"jumpPressed: {jumpPressed}  _isGrounded: {_isGrounded}");
            
            if (jumpPressed && _isGrounded)
            {
                _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }
        }

        private void OnDestroy()
        {
            if (_inputHandler != null)
            {
                _inputHandler.OnInputUpdated -= HandleInput;
            }
        }
    }
}