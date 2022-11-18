using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider ambientVolumeSlider;
    [SerializeField] private Slider SFXVolumeSlider;

    public void changeMusicVolume(float volume)
    {
        audioMixer.SetFloat("AmbientVolume", Mathf.Log10(volume) * 20);
    }

    public void changeSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }
}
