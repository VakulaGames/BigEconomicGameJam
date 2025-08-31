using System;
using DG.Tweening;
using UnityEngine;

namespace CORE
{
    [Serializable]
    public class TweenPlayList: IDisposable
    {
        [SerializeField] private TweenSetting[] _tweens = null;
        [SerializeField] private bool _ignoreTimeScale = false;
        
        private Sequence _sequence = null;

        public void Play(Action onComplete = null)
        {
            _sequence = DOTween.Sequence();
                
                if (_ignoreTimeScale)
                    _sequence.SetUpdate(true);

            foreach (var tweenSetting in _tweens)
            {
                AddTween(tweenSetting);
            }

            _sequence.OnComplete(() => onComplete?.Invoke());
        }

        private void AddTween(TweenSetting tweenSetting)
        {
            switch (tweenSetting.LaunchMethod)
            {
                case   TweenLaunchMethod.Apend:
                    _sequence.Append(tweenSetting.Tween);
                    break;
                    
                case TweenLaunchMethod.Inseart:
                    _sequence.Insert(tweenSetting.Delay, tweenSetting.Tween);
                    break;
                    
                case TweenLaunchMethod.Join:
                    _sequence.Join(tweenSetting.Tween);
                    break;
            }
        }

        public void Dispose()
        {
            if (_sequence != null)
            {
                _sequence.Kill();
                _sequence = null;
            }
        }
    }
}