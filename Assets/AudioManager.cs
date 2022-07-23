using System;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Serializable]
    private class AudioClip
    {
        public string name;
        public string[] tags;
        public AudioSource audio;
    }

    [SerializeField]
    private AudioClip[] audioClips;

    public void Play(string name)
    {
        Array.Find(audioClips, audioClip => audioClip.name == name).audio.Play();
    }

    public void PlayAtPoint(string name, Vector3 point)
    {
        AudioSource.PlayClipAtPoint(Array.Find(audioClips, audioClip => audioClip.name == name).audio.clip, point);
    }

    public void PlayRandomWithTag(string tag)
    {
        AudioClip[] tmp = Array.FindAll<AudioClip>(audioClips, audioClip => audioClip.tags.Contains(tag));
        int index = UnityEngine.Random.Range(0, tmp.Length);
        tmp[index].audio.Play();
    }

    public void Stop(string name)
    {
        Array.Find(audioClips, audioClip => audioClip.name == name).audio.Stop();
    }
}
