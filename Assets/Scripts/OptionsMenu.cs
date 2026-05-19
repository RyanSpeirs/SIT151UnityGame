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
       
    }

    public void OnEnable()
    {
        // Set initial values
        if (musicSlider != null && MusicManager.Instance != null)
        {
            musicSlider.value = MusicManager.Instance.MusicVolume;
        }

        if (sfxSlider != null && AudioManager.Instance != null)
        {
            sfxSlider.value = AudioManager.Instance.sfxVolume;
        }

        if (uiSlider != null && AudioManager.Instance != null)
        {
            uiSlider.value = AudioManager.Instance.uiVolume;
        }

    }

    public void OnMusicChanged(float value)
    {
        MusicManager.Instance.SetMusicVolume(value);
        MusicManager.Instance.SaveSettings();
    }

    public void OnSFXChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        AudioManager.Instance.SaveSettings();
    }

    public void OnUIChanged(float value)
    {
        AudioManager.Instance.SetUIVolume(value);
        AudioManager.Instance.SaveSettings();
    }

   

    void Awake()
    {
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(OnMusicChanged);
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        }

        if (uiSlider != null)
        {
            uiSlider.onValueChanged.RemoveAllListeners();
            uiSlider.onValueChanged.AddListener(OnUIChanged);
        }
    }
}