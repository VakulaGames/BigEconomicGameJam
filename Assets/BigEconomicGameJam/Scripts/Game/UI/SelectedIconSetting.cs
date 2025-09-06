using System;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace BigEconomicGameJam
{
    [Serializable]
    public struct SelectedIconSetting
    {
        [Constant("InteractableType")] public string Id;
        public GameObject GameObject;
    }
}