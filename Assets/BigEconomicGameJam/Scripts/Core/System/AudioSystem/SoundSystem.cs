using System.Collections.Generic;
using UnityEngine;

namespace CORE
{
    public class SoundSystem : MonoBehaviour
    {
        [SerializeField] private SoundConfig _soundConfig = null;
        [SerializeField] private AudioSourceComponent _source = null;

        private EventBinding<EventPlaySound> _eventPlaySound = null;
        private EventBinding<EventMuteSound> _eventMuteSound = null;
        private List<AudioSourceComponent> _sources = null;
        private Dictionary<SoundName, SoundData> _soundsDic = null;
        private bool _isMute = false;

        private Dictionary<SoundName, SoundData> SoundsDic
        {
            get
            {
                if (_soundsDic == null)
                {
                    _soundsDic = new Dictionary<SoundName, SoundData>();

                    foreach (var soundData in _soundConfig.SoundsData)
                    {
                        _soundsDic[soundData.Name] = soundData;
                    }
                }

                return _soundsDic;
            }
        }

        private void Start()
        {
            _eventPlaySound = new EventBinding<EventPlaySound>(PlaySound);
            EventBus<EventPlaySound>.Register(_eventPlaySound);

            _eventMuteSound = new EventBinding<EventMuteSound>(Mute);
            EventBus<EventMuteSound>.Register(_eventMuteSound);
        }

        private void Mute(EventMuteSound @event)
        {
            _isMute = @event.IsMute;
        }

        private void PlaySound(EventPlaySound sound)
        {
            if (_isMute)
                return;

            GetFreeSource().Play(SoundsDic[sound.SoundName]);
        }

        private AudioSourceComponent GetFreeSource()
        {
            if (_sources == null)
            {
                _sources = new List<AudioSourceComponent>();
                _sources.Add(_source);
                return _source;
            }

            foreach (AudioSourceComponent source in _sources)
            {
                if (!source.IsPlaying)
                {
                    return source;
                }
            }

            AudioSourceComponent newSource = Instantiate(_source, this.transform);
            _sources.Add(newSource);
            return newSource;
        }

        private void OnDestroy()
        {
            EventBus<EventPlaySound>.Unregister(_eventPlaySound);
        }
    }
}

