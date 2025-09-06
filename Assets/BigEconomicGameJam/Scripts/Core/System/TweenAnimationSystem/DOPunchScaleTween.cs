using DG.Tweening;
using UnityEngine;

namespace CORE
{
    public class DOPunchScaleTween: BaseTweenImplementaion
    {
        [SerializeField] private Transform _transform = null;
        [SerializeField] private Vector3 _punch;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _elasticity = 90f;

        public override Tween GetTweenImplementation() =>
            _transform.DOPunchScale(_punch, _duration, _vibrato, _elasticity);
    }
}