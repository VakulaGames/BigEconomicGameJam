using System;
using CORE;
using UnityEngine;
using UnityEngine.UI;

namespace BigEconomicGameJam
{
    public class ComputerWindow: UIWindow
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _buyButton;
        
        private CharacterService _characterService = null;
        private BuildingService _buildingService = null;
        
        private CharacterService CharacterService => _characterService != null? _characterService: ServiceLocator.GetService<CharacterService>();
        private BuildingService BuildingService => _buildingService != null? _buildingService: ServiceLocator.GetService<BuildingService>();

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
            _buyButton.onClick.AddListener(Buy);
        }

        public override void Show(Action onComplete)
        {
            this.gameObject.SetActive(true);
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

        private void Buy()
        {
            CharacterService.ResumeGame();
            var buildedObject = BuildingService.CreateObject("SawMachine");
            CharacterService.SetState(typeof(BuildingState), buildedObject);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
            _buyButton.onClick.RemoveListener(Buy);
        }
    }
}