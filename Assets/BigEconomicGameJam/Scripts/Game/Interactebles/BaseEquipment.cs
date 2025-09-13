using System;
using CORE;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class BaseEquipment: BaseInteractable, IBuildingObject
    {
        [SerializeField, Constant("UIWindows")] private string _window;
        
        private CharacterService _characterService = null;
        private UISystem _uiSystem = null;
        
        private CharacterService CharacterService => _characterService != null? _characterService: ServiceLocator.GetService<CharacterService>();
        private UISystem UISystem => _uiSystem != null? _uiSystem: ServiceLocator.GetService<UISystem>();
        
        public Vector3 Position 
        { 
            get => transform.position; 
            set => transform.position = value; 
        }
    
        public Vector2Int GridPosition { get; set; }
        public event Action<IBuildingObject> OnBuilt;
    
        // Вызовите этот метод после постройки
        public void NotifyBuilt()
        {
            OnBuilt?.Invoke(this);
        }
        
        public override void SetAction()
        {
            CharacterService.PauseGame();
            UISystem.OpenWindow(_window);
        }
    }
}