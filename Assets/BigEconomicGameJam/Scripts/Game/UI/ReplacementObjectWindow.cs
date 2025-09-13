using System;
using CORE;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace BigEconomicGameJam
{
    public abstract class ReplacementObjectWindow: UIWindow
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _replaceButton;
        
        private CharacterService _characterService = null;
        private BuildingService _buildingService = null;
        private BaseEquipment _sourceEquipment = null;
        
        protected CharacterService CharacterService => _characterService != null? _characterService: ServiceLocator.GetService<CharacterService>();
        protected BuildingService BuildingService => _buildingService != null? _buildingService: ServiceLocator.GetService<BuildingService>();
        
        protected virtual void Start()
        {
            _closeButton.onClick.AddListener(Close);
            _replaceButton.onClick.AddListener(Replace);
        }
        
        public override void Show(Object obj = null, Action onComplete = null)
        {
            this.gameObject.SetActive(true);

            if (obj is BaseEquipment baseEquipment)
            {
                _sourceEquipment = baseEquipment;
            }
            else
            {
                Debug.LogError($"object: {obj} is not BaseEquipment");
            }
            
            onComplete?.Invoke();
        }

        public override void Hide(Action onComplete)
        {
            this.gameObject.SetActive(false);
            onComplete?.Invoke();
        }
        
        private void Close()
        {
            CharacterService.ResumeGame();
        }
        
        private void Replace()
        {
            CharacterService.ResumeGame();
            CharacterService.SetState(typeof(BuildingState), _sourceEquipment);
        }
        
        protected virtual void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
            _replaceButton.onClick.RemoveListener(Replace);
        }
    }
}