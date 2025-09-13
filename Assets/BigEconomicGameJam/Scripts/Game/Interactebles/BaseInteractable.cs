using CORE;
using DG.Tweening;
using UnityEngine;

namespace BigEconomicGameJam
{
    public abstract class BaseInteractable: AbstractInteractable
    {
        [SerializeField] protected Rigidbody _rb;
        [SerializeField] protected Renderer[] _renders;
        [SerializeField] protected Collider _trigger;
        [SerializeField] protected Collider[] _colliders;
        
        protected Sequence _sequence = null;
        
        public Rigidbody Rigidbody => _rb;
        public Renderer[] Renderers => _renders;
        public Collider Trigger => _trigger;
        public Collider[] Colliders => _colliders;
        
        protected virtual void OnDestroy()
        {
            _sequence.SafeKill();
        }
    }
}