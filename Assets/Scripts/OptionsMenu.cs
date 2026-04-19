using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    void Start()
    {
        // Set initial values
        musicSlider.value = MusicManager.Instance.MusicVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;
        uiSlider.value = AudioManager.Instance.uiVolume;

        // Hook events
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        uiSlider.onValueChanged.AddListener(OnUIChanged);
    }

    public void OnMusicChanged(float value)
    {
        MusicManager.Instance.SetMusicVolume(value);
    }

    public void OnSFXChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void OnUIChanged(float value)
    {
        AudioManager.Instance.SetUIVolume(value);
    }

    public void Apply()
    {
        AudioManager.Instance.SaveSettings();
        MusicManager.Instance.SaveSettings();
    }
}