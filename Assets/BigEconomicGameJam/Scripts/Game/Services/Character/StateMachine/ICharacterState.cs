using System;

namespace BigEconomicGameJam
{
    public interface ICharacterState
    {
        public void Enter(Object obj);
        public void Update();
        public void Exit();
        public void HandleClick(MouseClickData clickData);
    }
}