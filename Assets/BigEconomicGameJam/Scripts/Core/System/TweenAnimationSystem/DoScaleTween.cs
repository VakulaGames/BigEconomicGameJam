using DG.Tweening;
using UnityEngine;

namespace CORE
{
    public class DoScaleTween: BaseTweenImplementaion
    {
        [SerializeField] private Transform _transform = null;
        [SerializeField] private float _scale = 1f;

        public override Tween GetTweenImplementation() =>
            _transform.DOScale(_scale, _duration);
    }
}