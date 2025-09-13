using UnityEngine;
using UnityEngine.UI;

namespace BigEconomicGameJam
{
    public class ComputerWindow: ReplacementObjectWindow
    {
        [SerializeField] private Button _buyButton;
        
        protected override void Start()
        {
            _buyButton.onClick.AddListener(Buy);
            
            base.Start();
        }

        private void Buy()
        {
            CharacterService.ResumeGame();
            var buildedObject = BuildingService.CreateObject("SawMachine");//todo
            CharacterService.SetState(typeof(BuildingState), buildedObject);
        }

        protected override void OnDestroy()
        {
            _buyButton.onClick.RemoveListener(Buy);
            
            base.OnDestroy();
        }
    }
}