using System;
using System.Collections.Generic;
using CORE;
using UnityEngine;
using Object = System.Object;

namespace BigEconomicGameJam
{
    public class StateMachine: MonoBehaviour
    {
        [SerializeReference, SubclassSelector] private IStateSetting[] _stateSettings;
        
        private ICharacterState _previesState;
        private ICharacterState _currentState;
        private Dictionary<Type, ICharacterState> _states;

        public void Init()
        {
            _states = new Dictionary<Type, ICharacterState>();

            foreach (var setting in _stateSettings)
            {
                _states[setting.Type] = setting.GetState();
            }
        }
        
        public void OnUpdate()
        {
            _currentState?.Update();
        }

        public void HandleClick(MouseClickData clickData)
        {
            _currentState?.HandleClick(clickData);
        }
        
        public void SetState(Type type, Object obj = null)
        {
            _previesState = _currentState;
            
            _currentState?.Exit();
            _currentState = _states[type];
            _currentState?.Enter(obj);
        }

        public void SetPreviosState()
        {
            if (_previesState == null)
                return;
            
            _currentState?.Exit();
            _currentState = _previesState;
            _currentState?.Enter(null);
        }
    }
}