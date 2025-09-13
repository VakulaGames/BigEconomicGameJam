using System;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class EmptyHandsStateSetting: IStateSetting
    {
        [SerializeField] private TakenObjectsContainer _objectsContainer;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private float _interactionDistance = 3f;
        
        public Type Type => typeof(EmptyHandsState);

        public TakenObjectsContainer ObjectsContainer => _objectsContainer;
        public LayerMask InteractionLayer => _interactionLayer;
        public float InteractionDistance => _interactionDistance;

        public ICharacterState GetState()
        {
            return new EmptyHandsState(this);
        }
    }
}