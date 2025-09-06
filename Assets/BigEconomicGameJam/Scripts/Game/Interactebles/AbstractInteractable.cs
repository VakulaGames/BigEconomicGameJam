using BigEconomicGameJam.Events;
using CORE;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace BigEconomicGameJam
{
    public abstract class AbstractInteractable: MonoBehaviour
    {
        [SerializeField, Constant("InteractableID")] private string _id;
        [SerializeField, Constant("InteractableType")] private string _interactableType;

        public string ID => _id;
        public string InteractableType => _interactableType;

        public bool Enabled { get; protected set; } = true;
        
        public abstract void SetAction();

        public virtual void Select()
        {
            
        }

        public virtual void UnSelect()
        {
            
        }
    }
}