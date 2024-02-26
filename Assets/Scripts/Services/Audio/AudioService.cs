using StaticData;
using UnityEngine;

namespace Services.Audio
{
    public class AudioService : IAudioService
    {
        private SoundsData _soundsData;
        private AudioSource _audioSource;
        
        public bool Muted
        {
            get => _audioSource.mute;
            set => _audioSource.mute = value;
        }

        public AudioService(SoundsData soundsData)
        {
            _soundsData = soundsData;
            _audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
            _audioSource.loop = true;
            Object.DontDestroyOnLoad(_audioSource);
        }

        public void PlayMusic()
        {
            _audioSource.clip = _soundsData.runningMusics[Random.Range(0, _soundsData.runningMusics.Length)];
            _audioSource.Play();
        }

        public void PlayBump()
        {
            _audioSource.PlayOneShot(_soundsData.bump);
        }

        public void PlayDing()
        {
            _audioSource.PlayOneShot(_soundsData.ding);
        }

        public void PlayFinish()
        {
            _audioSource.Stop();
            _audioSource.clip = _soundsData.finishMusics[Random.Range(0, _soundsData.finishMusics.Length)];
            _audioSource.Play();
        }
    }
}