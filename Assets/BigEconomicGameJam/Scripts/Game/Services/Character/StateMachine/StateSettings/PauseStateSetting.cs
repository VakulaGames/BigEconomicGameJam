using System;

namespace BigEconomicGameJam
{
    public class PauseStateSetting: IStateSetting
    {
        public Type Type => typeof(PauseState);
        
        public ICharacterState GetState()
        {
            return new PauseState(this);
        }
    }
}