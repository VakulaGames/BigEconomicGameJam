namespace CORE
{
    public struct EventMuteSound : IEvent
    {
        public bool IsMute;

        public EventMuteSound(bool isMute)
        {
            IsMute = isMute;
        }
    }
}
