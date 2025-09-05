using System;
using UnityEngine;

namespace BigEconomicGameJam
{
    [Serializable]
    public class PlayerInputData
    {
        public Vector2 Movement;
        public Vector2 MouseLook;
        public bool IsRunning;
        public bool JumpPressed;
        
        public void Reset()
        {
            Movement = Vector2.zero;
            MouseLook = Vector2.zero;
            IsRunning = false;
            JumpPressed = false;
        }
    }
}