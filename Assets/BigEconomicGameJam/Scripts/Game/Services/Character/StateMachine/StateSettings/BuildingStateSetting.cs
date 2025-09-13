using System;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class BuildingStateSetting: IStateSetting
    {
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private float _interactionDistance = 3f;
        
        public Type Type => typeof(BuildingState);
        public LayerMask InteractionLayer => _interactionLayer;
        public float InteractionDistance => _interactionDistance;
        
        public ICharacterState GetState()
        {
            return new BuildingState(this);
        }
    }
}