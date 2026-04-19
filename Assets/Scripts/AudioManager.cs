using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioSource musicSource;
    [Range(0f, 1f)] public float musicVolume = 0.3f;

    [Header("SFX")]
    public AudioSource sfxPrefab; // prefab or reference AudioSource
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("UI")]
    [Range(0f, 1f)] public float uiVolume = 1f;

    // Internal fade control
    private float musicFadeMultiplier = 1f;
    private Coroutine fadeRoutine;

    // Save keys
    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";
    private const string UI_KEY = "UIVolume";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        ApplyMusicVolume();
    }

    // ------------------------
    // MUSIC
    // ------------------------

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null || clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = loop;
        ApplyMusicVolume();
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    // ------------------------
    // SFX
    // ------------------------

    public void PlaySFX(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip == null || sfxPrefab == null) return;

        AudioSource source = Instantiate(sfxPrefab, transform);
        source.clip = clip;
        source.volume = sfxVolume * volumeMultiplier;
        source.Play();

        Destroy(source.gameObject, clip.length);
    }

    public void PlayUISound(AudioClip clip)
    {
        PlaySFX(clip, uiVolume);
    }

    // ------------------------
    // VOLUME CONTROL
    // ------------------------

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        ApplyMusicVolume();
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = Mathf.Clamp01(value);
    }

    public void SetUIVolume(float value)
    {
        uiVolume = Mathf.Clamp01(value);
    }

    private void ApplyMusicVolume()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume * musicFadeMultiplier;
    }

    // ------------------------
    // FADE SYSTEM
    // ------------------------

    public void FadeMusic(float targetMultiplier, float duration)
    {
        if (musicSource == null) return;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeCoroutine(targetMultiplier, duration));
    }

    private IEnumerator FadeCoroutine(float targetMultiplier, float duration)
    {
        float start = musicFadeMultiplier;
        float time = 0f;

        while (time < duration)
        {
            musicFadeMultiplier = Mathf.Lerp(start, targetMultiplier, time / duration);
            ApplyMusicVolume();

            time += Time.deltaTime;
            yield return null;
        }

        musicFadeMultiplier = targetMultiplier;
        ApplyMusicVolume();
    }

    // ------------------------
    // SAVE / LOAD
    // ------------------------

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MUSIC_KEY, musicVolume);
        PlayerPrefs.SetFloat(SFX_KEY, sfxVolume);
        PlayerPrefs.SetFloat(UI_KEY, uiVolume);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 0.3f);
        sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        uiVolume = PlayerPrefs.GetFloat(UI_KEY, 1f);
    }
}