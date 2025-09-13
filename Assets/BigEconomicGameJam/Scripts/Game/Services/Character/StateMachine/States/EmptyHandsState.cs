using BigEconomicGameJam.Events;
using CORE;
using UnityEngine;
using Object = System.Object;

namespace BigEconomicGameJam
{
    public class EmptyHandsState : BaseCharacterState
    {
        private float _interactionDistance;
        private LayerMask _interactionLayer;
        private TakenObjectsContainer _objectsContainer;
        private CharacterService _characterService = null;
        private Transform _cameraTransform = null;
        
        private CharacterService CharacterService => _characterService != null? _characterService: ServiceLocator.GetService<CharacterService>();
        private Transform CameraTransform => _cameraTransform != null? _cameraTransform: ServiceLocator.GetService<CameraService>().Camera.transform;
        
        public AbstractInteractable SelectedInteractable { get; private set; }
        public TakedIntaractable TakenInteractable { get; private set; }

        public EmptyHandsState(IStateSetting stateSetting): base(stateSetting)
        {
            var setting = stateSetting as EmptyHandsStateSetting; 
            
            _objectsContainer = setting.ObjectsContainer;
            _interactionLayer = setting.InteractionLayer;
            _interactionDistance = setting.InteractionDistance;
        }

        public override void Enter(Object obj)
        {
            SelectedInteractable = null;
            TakenInteractable = null;
        }

        public override void Update()
        {
            if (CharacterService.IsPaused) return;
            DetectInteractableObjects();
        }

        public override void Exit()
        {
            if (SelectedInteractable != null)
            {
                SelectedInteractable.UnSelect();
                EventBus<EventUnSelectInteractable>.Raise(new EventUnSelectInteractable(
                    SelectedInteractable.InteractableType));
                SelectedInteractable = null;
            }
        }

        public override void HandleClick(MouseClickData clickData)
        {
            if (clickData.LeftButtonDown)
            {
                if (SelectedInteractable != null)
                {
                    SelectedInteractable.SetAction();
            
                    if (SelectedInteractable is TakedIntaractable takedIntaractable)
                    {
                        TakenInteractable = takedIntaractable;
                        _objectsContainer.Take(takedIntaractable);
                        EventBus<EventUnSelectInteractable>.Raise(new EventUnSelectInteractable(
                            takedIntaractable.InteractableType));
                
                        CharacterService.SetState(typeof(HoldingObjectState), takedIntaractable);
                    }
                }
            }
        }

        private void DetectInteractableObjects()
        {
            if (TakenInteractable != null) return;
            
            Ray ray = new Ray(CameraTransform.position, CameraTransform.forward);
            RaycastHit hit;
    
            if (Physics.Raycast(ray, out hit, _interactionDistance, _interactionLayer))
            {
                if (hit.collider.TryGetComponent<AbstractInteractable>(out AbstractInteractable interactable) && interactable.Enabled)
                {
                    if (SelectedInteractable != interactable)
                    {
                        if (SelectedInteractable != null)
                        {
                            SelectedInteractable.UnSelect();
                            EventBus<EventUnSelectInteractable>.Raise(new EventUnSelectInteractable(
                                SelectedInteractable.InteractableType));
                        }
                
                        interactable.Select();
                        EventBus<EventSelectInteractable>.Raise(new EventSelectInteractable(
                            interactable.InteractableType));
                        SelectedInteractable = interactable;
                    }
                    return;
                }
            }
    
            if (SelectedInteractable != null)
            {
                SelectedInteractable.UnSelect();
                EventBus<EventUnSelectInteractable>.Raise(new EventUnSelectInteractable(
                    SelectedInteractable.InteractableType));
                SelectedInteractable = null;
            }
        }
    }
}