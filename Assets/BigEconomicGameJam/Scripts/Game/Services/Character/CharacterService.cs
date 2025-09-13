using System;
using CORE;
using CORE.CONST_SELECTOR;
using UnityEngine;
using Object = System.Object;

namespace BigEconomicGameJam
{
    public class CharacterService : AbstractMonoService, IInitializable, IUpdatable
    {
        [SerializeField] private FirstPersonController _controller;
        [SerializeField] private StateMachine _stateMachine;
        
        [Header("Interaction Settings")]
        [SerializeField] private LayerMask _interactionLayer = Physics.DefaultRaycastLayers;
        
        [Header("UI Windows")]
        [SerializeField, Constant("UIWindows")] private string _mainMenuWindow = "MainMenu";
        [SerializeField, Constant("UIWindows")] private string _gamePlayWindow = "GamePlayWindow";
        
        private Lazy<UISystem> _uiSystem = null;
        private Lazy<InputHandler> _inputHandler = null;
        
        public override Type RegisterType => typeof(CharacterService);
        public bool IsPaused { get; private set; } = true;
        
        public void Init()
        {
            _uiSystem = new Lazy<UISystem>(() => ServiceLocator.GetService<UISystem>());
            _inputHandler = new Lazy<InputHandler>(() => ServiceLocator.GetService<InputHandler>());
            
            _controller.Init();
            _stateMachine.Init();

            _inputHandler.Value.OnPause += SetPause;
            _inputHandler.Value.OnMouseClick += HandleClick;
        }

        public void OnUpdate()
        {
            _controller.OnUpdate();
            _stateMachine.OnUpdate();
        }

        private void HandleClick(MouseClickData clickData)
        {
            if (IsPaused) return;

            _stateMachine.HandleClick(clickData);
        }

        public void SetPause()
        {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        public void PauseGame()
        {
            IsPaused = true;
            
            _controller.SetEnableControl(false);
            _stateMachine.SetState(typeof(PauseState));
            _uiSystem.Value.OpenWindow(_mainMenuWindow);
        }

        public void ResumeGame()
        {
            IsPaused = false;
            
            _stateMachine.SetPreviosState();
            
            _uiSystem.Value.OpenWindow(_gamePlayWindow, () =>
            {
                _controller.SetEnableControl(true);
            });
        }

        private void OnDestroy()
        {
            if (_inputHandler?.Value != null)
            {
                _inputHandler.Value.OnPause -= SetPause;
                _inputHandler.Value.OnMouseClick -= HandleClick;
            }
        }

        public void SetState(Type type, Object obj = null)
        {
            _stateMachine.SetState(type, obj);
        }
    }
}