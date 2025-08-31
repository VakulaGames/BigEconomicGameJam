using System;
using DG.Tweening;
using UnityEngine;

namespace CORE
{
    [Serializable]
    public class DoAnchorPosTween: BaseTweenImplementaion
    {
        [SerializeField] private RectTransform _rectTransform = null;
        [SerializeField] private Vector2 _target;

        public override Tween GetTweenImplementation()=> _rectTransform.DOAnchorPos(_target, _duration);
    }
}