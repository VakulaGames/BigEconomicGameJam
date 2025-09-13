using System;
using System.Linq;
using CORE;
using UnityEngine;

namespace BigEconomicGameJam
{
    public class BuildingService : AbstractMonoService, IInitializable
    {
        [SerializeField] private BaseEquipment[] _prefabs;
        
        public override Type RegisterType => typeof(BuildingService);

        public void Init()
        {
        }

        public IBuildingObject CreateObject(string id)
        {
            var prefab = _prefabs.First(x => x.ID == id);
            var instance = Instantiate(prefab);
            instance.SetPurchased(false);
            return instance;
        }
    }
}
