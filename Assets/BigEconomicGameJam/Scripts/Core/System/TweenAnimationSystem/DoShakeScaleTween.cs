using DG.Tweening;
using UnityEngine;

namespace CORE
{
    public class DoShakeScaleTween: BaseTweenImplementaion
    {
        [SerializeField] private Transform _transform = null;
        [SerializeField] private float _strength = 1f;
        [SerializeField] private int _vibrato = 10;
        [SerializeField] private float _randomness = 90f;
        [SerializeField] private bool _fadeOut = true;

        public override Tween GetTweenImplementation() =>
            _transform.DOShakeScale(_duration, _strength, _vibrato, _randomness, _fadeOut);
    }
}