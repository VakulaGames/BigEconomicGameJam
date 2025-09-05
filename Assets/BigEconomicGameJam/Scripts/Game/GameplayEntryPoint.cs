using CORE;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class GameplayEntryPoint: MonoBehaviour
    {
        [SerializeField] private ServiceRegister _serviceRegister = null;

        [SerializeField, Constant("UIWindows")] private string _startWindow = "MainMenu";
        
        private async void Awake()
        {
            await _serviceRegister.RegisterServises();
            
            ServiceLocator.GetService<UISystem>().OpenWindow(_startWindow);
        }
    }
}