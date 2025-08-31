using System.Collections.Generic;
using UnityEngine;

namespace CORE
{
    [CreateAssetMenu(menuName = "Config/Sound Config")]
    public class SoundConfig: ScriptableObject
    {
        public List<SoundData> SoundsData;
    }
}
