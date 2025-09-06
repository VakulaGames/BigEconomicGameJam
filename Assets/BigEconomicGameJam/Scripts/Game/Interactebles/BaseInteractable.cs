using CORE;
using DG.Tweening;

namespace BigEconomicGameJam
{
    public abstract class BaseInteractable: AbstractInteractable
    {
        protected Sequence _sequence = null;
        
        protected virtual void OnDestroy()
        {
            _sequence.SafeKill();
        }
    }
}