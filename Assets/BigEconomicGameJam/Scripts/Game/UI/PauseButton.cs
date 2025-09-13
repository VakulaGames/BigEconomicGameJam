using System;
using CORE;
using UnityEngine;
using UnityEngine.UI;

namespace BigEconomicGameJam
{
    public class PauseButton: MonoBehaviour
    {
        [SerializeField] private Button _menuButton;

        private Lazy<CharacterService> _characterService = null;
        
        private void Start()
        {
            _characterService = new Lazy<CharacterService>(
                () => ServiceLocator.GetService<CharacterService>());
            
            _menuButton.onClick.AddListener(PauseGame);
        }

        private void PauseGame()
        {
            //_characterService.Value.SetPause();
        }

        private void OnDestroy()
        {
            _menuButton.onClick.RemoveListener(PauseGame);
        }
    }
}