using System;

namespace BigEconomicGameJam
{
    public class HoldingObjectStateSetting: IStateSetting
    {
        public Type Type => typeof(HoldingObjectState);
        
        public ICharacterState GetState()
        {
            return new HoldingObjectState();
        }
    }
}