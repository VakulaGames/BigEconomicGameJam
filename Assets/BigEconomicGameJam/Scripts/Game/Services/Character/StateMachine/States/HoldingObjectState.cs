using CORE;
using UnityEngine;
using Object = System.Object;

namespace BigEconomicGameJam
{
    public class HoldingObjectState : BaseCharacterState
    {
        private CharacterService _characterService = null;
        private Transform _cameraTransform = null;
        
        private CharacterService CharacterService => _characterService != null? _characterService: ServiceLocator.GetService<CharacterService>();
        private Transform CameraTransform => _cameraTransform != null? _cameraTransform: ServiceLocator.GetService<CameraService>().Camera.transform;
        
        public TakedIntaractable HeldObject { get; private set; }

        public override void Enter(Object obj)
        {
            if (obj == null)
                return;
            
            if (obj is TakedIntaractable intaractable)
            {
                HeldObject = intaractable;
            }
            else
            {
                Debug.LogError($"object: {obj} is not TakedIntaractable");
            }
        }

        public override void Update()
        {
            
        }

        public override void Exit() { }

        public override void HandleClick()
        {
            if (HeldObject != null)
            {
                HeldObject.Push(CameraTransform.forward);
                HeldObject = null;
                CharacterService.SetState(typeof(EmptyHandsState));
            }
        }
    }
}