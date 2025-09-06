using UnityEngine;

namespace BigEconomicGameJam
{
    public abstract class TakedIntaractable: BaseInteractable
    {
        [SerializeField] protected Rigidbody _rb;
        [SerializeField] private float _pushForce = 5f;

        public Rigidbody Rigidbody => _rb;

        public override void SetAction()
        {
            _rb.isKinematic = true;
        }

        public virtual void Push(Vector3 direction)
        {
            this.transform.SetParent(null);
            
            _rb.isKinematic = false;
            _rb.AddForce(direction * _pushForce, ForceMode.Impulse);
        }
    }
}