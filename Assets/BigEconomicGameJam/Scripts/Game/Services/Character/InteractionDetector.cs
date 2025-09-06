using BigEconomicGameJam.Events;
using CORE;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class InteractionDetector: MonoBehaviour
    {
        [SerializeField] private float _interactionDistance = 3f;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private TakenObjectsContainer _objectsContainer;
    
        private bool _isEnabled = false;
        
        public AbstractInteractable SelectedInteractable { get; private set; }
        public TakedIntaractable TakenInteractable { get; private set; }
    
        public void OnUpdate()
        {
            if (!_isEnabled) return;
            
            DetectInteractableObjects();
        }

        public void SetEnableControl(bool enable)
        {
            _isEnabled = enable;
        }

        public void OnClick()
        {
            if (TakenInteractable != null)
            {
                TakenInteractable.Push(transform.forward);
                TakenInteractable = null;
            }
            else
            {
                if (SelectedInteractable != null)
                {
                    SelectedInteractable.SetAction();
                    
                    if (SelectedInteractable is TakedIntaractable takedIntaractable)
                    {
                        TakenInteractable = takedIntaractable;
                        _objectsContainer.Take(takedIntaractable);
                        EventBus<EventUnSelectInteractable>.Raise(new EventUnSelectInteractable(takedIntaractable.InteractableType));
                    }
                }
            }
        }

        private void DetectInteractableObjects()
        {
            if (TakenInteractable != null)
                return;
            
            Ray ray = new Ray(transform.position, transform.forward);
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
                            EventBus<EventUnSelectInteractable>.Raise(new EventUnSelectInteractable(SelectedInteractable.InteractableType));
                        }
                    
                        interactable.Select();
                        EventBus<EventSelectInteractable>.Raise(new EventSelectInteractable(interactable.InteractableType));
                        SelectedInteractable = interactable;
                    }
                    return;
                }
            }
        
            if (SelectedInteractable != null)
            {
                SelectedInteractable.UnSelect();
                EventBus<EventUnSelectInteractable>.Raise(new EventUnSelectInteractable(SelectedInteractable.InteractableType));
                SelectedInteractable = null;
            }
        }
    }
}