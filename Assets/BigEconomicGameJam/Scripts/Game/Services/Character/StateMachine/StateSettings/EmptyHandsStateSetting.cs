using System;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class EmptyHandsStateSetting: IStateSetting
    {
        [SerializeField] private TakenObjectsContainer _objectsContainer;
        [SerializeField] private LayerMask _interactionLayer;
        
        public Type Type => typeof(EmptyHandsState);
        
        public ICharacterState GetState()
        {
            return new EmptyHandsState(_objectsContainer, _interactionLayer);
        }
    }
}