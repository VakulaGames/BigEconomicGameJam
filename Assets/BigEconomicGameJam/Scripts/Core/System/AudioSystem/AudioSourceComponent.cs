using UnityEngine;

namespace CORE
{
    public class AudioSourceComponent : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource = null;

        public bool IsPlaying => _audioSource.isPlaying;

        public void Play(SoundData soundData)
        {
            _audioSource.clip = soundData.AudioClip;
            _audioSource.volume = soundData.Volume;
            _audioSource.Play();
        }
    }
}

