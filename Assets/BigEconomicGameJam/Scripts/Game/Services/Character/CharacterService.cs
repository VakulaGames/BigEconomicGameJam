using System;
using CORE;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class CharacterService: AbstractMonoService, IInitializable, IUpdatable
    {
        [SerializeField] private FirstPersonController _controller;
        
        [Header("UI Windows")]
        [SerializeField, Constant("UIWindows")] private string _mainMenuWindow = "MainMenu";
        [SerializeField, Constant("UIWindows")] private string _gamePlayWindow = "GamePlayWindow";
        
        private Lazy<UISystem> _uiSystem = null;
        private Lazy<InputHandler> _inputHandler = null;
        
        public override Type RegisterType => typeof(CharacterService);
        public bool IsPause { get; private set; } = true;
        
        public void Init()
        {
            _uiSystem = new Lazy<UISystem>(() => ServiceLocator.GetService<UISystem>());
            _inputHandler = new Lazy<InputHandler>(() => ServiceLocator.GetService<InputHandler>());
            
            _controller.Init();

            _inputHandler.Value.OnPause += SetPause;
        }

        public void OnUpdate()
        {
            _controller.OnUpdate();
        }

        public void SetPause()
        {
            if (IsPause)
            {
                PlayGame();
                IsPause = false;
            }
            else
            {
                Pause();
                IsPause = true;
            }
        }

        private void PlayGame()
        {
            _uiSystem.Value.OpenWindow(_gamePlayWindow, () =>
            {
                _controller.SetEnableControl(true);
            });
        }

        private void Pause()
        {
            _controller.SetEnableControl(false);
            
            _uiSystem.Value.OpenWindow(_mainMenuWindow);
        }

        private void OnDestroy()
        {
            _inputHandler.Value.OnPause -= SetPause;
        }
    }
}