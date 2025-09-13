using System;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class BuildingStateSetting: IStateSetting
    {
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private float _interactionDistance = 3f;
        [SerializeField, Constant("UIWindows")] private string _window;
        
        public Type Type => typeof(BuildingState);
        public LayerMask InteractionLayer => _interactionLayer;
        public float InteractionDistance => _interactionDistance;
        public string WindowID => _window;
        
        public ICharacterState GetState()
        {
            return new BuildingState(this);
        }
    }
}