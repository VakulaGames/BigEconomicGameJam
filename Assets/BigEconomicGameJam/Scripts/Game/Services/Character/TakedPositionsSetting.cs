using System;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace BigEconomicGameJam
{
    [Serializable]
    public struct TakedPositionsSetting
    {
        [Constant("InteractableID")] public string ID;
        public Transform Transform;
    }
}