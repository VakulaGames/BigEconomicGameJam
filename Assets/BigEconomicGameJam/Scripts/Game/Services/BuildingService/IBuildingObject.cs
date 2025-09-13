namespace BigEconomicGameJam
{
    public interface IBuildingObject
    {
        string ID { get; }
        void NotifyBuilt();
    }
}