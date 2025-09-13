
using System;

namespace BigEconomicGameJam
{
    public abstract class BaseCharacterState : ICharacterState
    {
        public BaseCharacterState(IStateSetting setting){}
        
        public abstract void Enter(Object obj);
        public abstract void Update();
        public abstract void Exit();
        public abstract void HandleClick(MouseClickData clickData);
    }
}