using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace CORE
{
    [Serializable]
    public class TweenSetting
    {
        [field: SerializeField] public TweenLaunchMethod LaunchMethod { get; private set; }
        [field: SerializeField] public float Delay { get; private set; }
        
        
        [SerializeReference, SubclassSelector] private ITweenImplementation _tweenImplementation = null;

        public Tween Tween => _tweenImplementation.GetTween();
    }
}