using System;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace CORE
{
    public abstract class UIWindow: MonoBehaviour
    {
        [field: SerializeField, Constant("UIWindows")] public string ID { get; protected set; }
        public abstract void Show(Action onComplete = null);
        public abstract void Hide(Action onComplete = null);
    }
}