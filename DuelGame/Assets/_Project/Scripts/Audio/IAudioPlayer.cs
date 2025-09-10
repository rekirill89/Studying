using UnityEngine;

namespace DuelGame
{
    public interface IAudioPlayer
    {
        public void Play(AudioClip clip);

        public void PlayOneShot(AudioClip clip);

        public void PlayOneShot(AudioClip clip, float volume);
    }
}