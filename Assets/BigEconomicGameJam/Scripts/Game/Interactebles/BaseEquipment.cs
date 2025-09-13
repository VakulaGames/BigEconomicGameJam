using System;
using CORE;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class BaseEquipment: BaseInteractable, IBuildingObject
    {
        [SerializeField, Constant("UIWindows")] private string _window;
        [SerializeField] private bool _isAdditionalTrigger = false;
        
        private CharacterService _characterService = null;
        private UISystem _uiSystem = null;
        private int _triggeredCounter = 0;
        
        private CharacterService CharacterService => _characterService != null? _characterService: ServiceLocator.GetService<CharacterService>();
        private UISystem UISystem => _uiSystem != null? _uiSystem: ServiceLocator.GetService<UISystem>();
        public bool IsPurchased { get; private set; } = true;
        
        public void SetPurchased(bool isPurchased)
        {
            IsPurchased = isPurchased;
        }
        
        public void StartBuilding()
        {
            if (Colliders != null && Colliders.Length > 0)
            {
                foreach (var collider in Colliders)
                {
                    collider.enabled = false;
                }
            }

            Trigger.enabled = true;
            Trigger.isTrigger = true;
        }
        
        public void NotifyBuilt()
        {
            Trigger.isTrigger = false;

            if (_isAdditionalTrigger)
                Trigger.enabled = false;
            
            if (Colliders != null && Colliders.Length > 0)
            {
                foreach (var collider in Colliders)
                {
                    collider.enabled = true;
                }
            }

            SetPurchased(true);
        }
        
        public override void SetAction()
        {
            CharacterService.Pause();
            UISystem.OpenWindow(_window, this);
        }
        
        public bool CanBuild()
        {
            return _triggeredCounter == 0;
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Interactable"))
            {
                _triggeredCounter++;
                Debug.Log($"On Enter. count {_triggeredCounter}");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Interactable"))
            {
                _triggeredCounter--;
                Debug.Log($"On Exit. count {_triggeredCounter}");
            }
        }
    }
}