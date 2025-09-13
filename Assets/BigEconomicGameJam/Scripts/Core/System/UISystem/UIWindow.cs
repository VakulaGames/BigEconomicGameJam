using System;
using CORE.CONST_SELECTOR;
using UnityEngine;
using Object = System.Object;

namespace CORE
{
    public abstract class UIWindow: MonoBehaviour
    {
        [field: SerializeField, Constant("UIWindows")] public string ID { get; protected set; }
        public abstract void Show(Object obj = null, Action onComplete = null);
        public abstract void Hide(Action onComplete = null);
    }
}