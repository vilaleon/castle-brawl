using System;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    [Serializable]
    private class AudioClip
    {
        public string name;
        public AudioSource clip;
    }

    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip[] SFXClips;

    public void PlayMusic(string name)
    {
        Array.Find(musicClips, sound => sound.name == name).clip.Play();
    }

    public void StopMusic(string name)
    {
        Array.Find(musicClips, sound => sound.name == name).clip.Stop();
    }

    public void StopAllMusic()
    {
        foreach (AudioClip musicClip in musicClips)
        {
            musicClip.clip.Stop();
        }
    }

    public void PlaySFX(string name)
    {
        Array.Find(SFXClips, sound => sound.name == name).clip.Play();
    }
}
