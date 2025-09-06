using CORE;

namespace BigEconomicGameJam.Events
{
    public struct EventUnSelectInteractable: IEvent
    {
        public string ID;

        public EventUnSelectInteractable(string id)
        {
            ID = id;
        }
    }
}