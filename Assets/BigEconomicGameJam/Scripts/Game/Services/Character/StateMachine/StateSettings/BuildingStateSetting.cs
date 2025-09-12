using System;

namespace BigEconomicGameJam
{
    public class BuildingStateSetting: IStateSetting
    {
        public Type Type => typeof(BuildingState);
        
        public ICharacterState GetState()
        {
            return new BuildingState();
        }
    }
}