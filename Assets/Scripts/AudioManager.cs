using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("SFX")]
    public AudioSource sfxPrefab;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("UI Sounds")]
    public AudioSource uiSfxPrefab;
    [Range(0f, 1f)] public float uiVolume = 1f;

    private const string SFX_KEY = "SFXVolume";
    private const string UI_KEY = "UIVolume";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    
   

    // ------------------------
    // SFX Control
    // ------------------------
    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
    }

    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip == null || sfxPrefab == null) return;

        AudioSource source = Instantiate(sfxPrefab, transform);
        source.clip = clip;
        source.volume = sfxVolume * volumeMultiplier;
        source.Play();

        Destroy(source.gameObject, clip.length);
    }

    // ------------------------
    // UI Sounds
    // ------------------------
    public void SetUIVolume(float value)
    {
        uiVolume = Mathf.Clamp01(value);
    }

    public void PlayUISound(AudioClip clip)
    {
        PlaySFX(clip, uiVolume);
    }

    //------------------------
    //  Save Settings
    //------------------------
    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(UI_KEY, uiVolume);
        PlayerPrefs.SetFloat(SFX_KEY, sfxVolume);
        PlayerPrefs.Save();
    }
}