using System;

namespace BigEconomicGameJam
{
    public interface IStateSetting
    {
        public Type Type { get; }
        public ICharacterState GetState();
    }
}