using System.Linq;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class TakenObjectsContainer: MonoBehaviour
    {
        [SerializeField] private TakedPositionsSetting[] _settings;
        
        public void Take(TakedIntaractable takedIntaractable)
        {
            Transform target = _settings.First(x => x.ID == takedIntaractable.ID).Transform;

            Transform source = takedIntaractable.transform;
            source.SetParent(target);
            source.localScale = Vector3.one;
            source.localRotation = Quaternion.identity;
            source.localPosition = Vector3.zero;
        }
    }
}