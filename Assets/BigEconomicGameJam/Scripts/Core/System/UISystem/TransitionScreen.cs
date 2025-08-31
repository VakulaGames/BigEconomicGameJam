using System;
using UnityEngine;

namespace CORE
{
    public class TransitionScreen: AbstractMonoService
    {
        [SerializeField] private GameObject _blockPanel = null;
        [SerializeField] private TweenPlayList _showAnimation = null;
        [SerializeField] private TweenPlayList _hideAnimation = null;
        
        public override Type RegisterType => typeof(TransitionScreen);

        public void Show(Action onComplete = null)
        {
            _blockPanel.SetActive(true);
            _showAnimation.Play(onComplete);
        }
        
        public void Hide(Action onComplete = null)
        {
            _hideAnimation.Play(() =>
            {
                onComplete?.Invoke();
                _blockPanel.SetActive(false);
            });
        }
    }
}