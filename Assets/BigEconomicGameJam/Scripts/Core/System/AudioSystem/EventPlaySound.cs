namespace CORE
{
    public struct EventPlaySound: IEvent
    {
        public SoundName SoundName;

        public EventPlaySound(SoundName soundName)
        {
            SoundName = soundName;
        }
    }
}
