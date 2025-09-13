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

            _previesState = _states[typeof(EmptyHandsState)];
            _currentState = _states[typeof(PauseState)];
            _currentState?.Enter(null);
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
            
            Debug.Log($"Preview state: {_previesState.GetType()} enter state: {_currentState.GetType()}");
        }

        public void SetPreviosState()
        {
            _currentState?.Exit();
            _currentState = _previesState;
            _currentState?.Enter(null);
            
            Debug.Log($"Preview state: {_previesState.GetType()} enter state: {_currentState.GetType()}");
        }
    }
}