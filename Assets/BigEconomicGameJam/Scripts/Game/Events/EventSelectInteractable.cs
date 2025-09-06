using CORE;

namespace BigEconomicGameJam.Events
{
    public struct EventSelectInteractable: IEvent
    {
        public string ID;

        public EventSelectInteractable(string id)
        {
            ID = id;
        }
    }
}