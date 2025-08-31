using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CORE
{
    public abstract class BaseTweenImplementaion: ITweenImplementation
    {
        protected float _duration;
        
        [SerializeField] private bool _isEase;
        [SerializeField, ShowIf(nameof(_isEase))] private Ease _ease;
        [SerializeField, HideIf(nameof(_isEase))] private AnimationCurve _curve;

        public abstract Tween GetTweenImplementation();
        
        public Tween GetTween()
        {
            if (_isEase)
            {
                return GetTweenImplementation().SetEase(_ease);
            }
            else
            {
                return GetTweenImplementation().SetEase(_curve);
            }
        }
    }
}