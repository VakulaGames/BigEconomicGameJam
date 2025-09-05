using System;
using CORE;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class GamePlayWindow: UIWindow
    {
        [SerializeField] private TweenPlayList _showAnimation;
        [SerializeField] private TweenPlayList _hideAnimation;

        public override void Show(Action onComplete)
        {
            this.gameObject.SetActive(true);
            _showAnimation.Play(onComplete);
        }

        public override void Hide(Action onComplete)
        {
            _hideAnimation.Play(() =>
            {
                this.gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }
    }
}