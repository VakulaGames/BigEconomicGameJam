using System;
using UnityEngine;

namespace BigEconomicGameJam
{
    public interface IBuildingObject
    {
        string ID { get; }
        Vector3 Position { get; set; }
        Vector2Int GridPosition { get; set; }
        event Action<IBuildingObject> OnBuilt;
        void NotifyBuilt();
    }
}