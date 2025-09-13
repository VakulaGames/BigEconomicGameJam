using System;
using CORE;
using Object = System.Object;

namespace BigEconomicGameJam
{
    public class MainMenuWindow: UIWindow
    {
        public override void Show(Object obj = null, Action onComplete = null)
        {
            this.gameObject.SetActive(true);
            onComplete?.Invoke();
        }

        public override void Hide(Action onComplete)
        {
            this.gameObject.SetActive(false);
            onComplete?.Invoke();
        }
    }
}