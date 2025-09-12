
using System;

namespace BigEconomicGameJam
{
    public abstract class BaseCharacterState : ICharacterState
    {
        public abstract void Enter(Object obj);
        public abstract void Update();
        public abstract void Exit();
        public abstract void HandleClick();
    }
}