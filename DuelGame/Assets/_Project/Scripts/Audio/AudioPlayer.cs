using System;
using UnityEngine;

namespace DuelGame
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioPlayer : MonoBehaviour, IAudioPlayer
    {
        private AudioSource _audioSource;

        public void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            
            DontDestroyOnLoad(_audioSource);
        }

        public void Play(AudioClip clip)
        {
            _audioSource.clip = clip;
            _audioSource.loop = true;
            _audioSource.Play();
        }

        public void PlayOneShot(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }
        
        public void PlayOneShot(AudioClip clip, float volume)
        {
            _audioSource.PlayOneShot(clip, volume);
        }
    }
}