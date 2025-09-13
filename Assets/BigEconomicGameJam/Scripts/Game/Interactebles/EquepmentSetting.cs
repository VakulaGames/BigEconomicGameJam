using System;
using CORE.CONST_SELECTOR;
using UnityEngine;

namespace BigEconomicGameJam
{
    [Serializable]
    public class EquepmentSetting
    {
        [SerializeField, Constant("InteractableID")] private string _id;

        public string ID => _id;
    }
}