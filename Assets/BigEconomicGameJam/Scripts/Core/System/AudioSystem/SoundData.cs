using System;
using UnityEngine;

namespace CORE
{
    [Serializable]
    public struct SoundData
    {
        public SoundName Name;
        public AudioClip AudioClip;
        public float Volume;
    }
}
