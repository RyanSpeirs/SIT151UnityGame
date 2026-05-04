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
        musicSlider.value = MusicManager.Instance.MusicVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;
        uiSlider.value = AudioManager.Instance.uiVolume;

      
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
        musicSlider.onValueChanged.RemoveAllListeners();
        sfxSlider.onValueChanged.RemoveAllListeners();  
        uiSlider.onValueChanged.RemoveAllListeners();   

        // Hook events
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        uiSlider.onValueChanged.AddListener(OnUIChanged);
    }
}