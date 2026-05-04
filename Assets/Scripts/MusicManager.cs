using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header ("Music Sources")]
    public AudioSource musicSource;
    public AudioSource gameplaySouce;
    public AudioSource pauseSource;

    [Header("Track Balance")]
    public float defaultMusicMultiplier = 1f;
    public float gameplayMultiplier = 0.8f;
    public float pauseMultiplier = 1f;
    public float gameOverMultiplier = 1.2f;
    public float mainmenuMultiplier = 1.0f;


    public AudioClip mainmenuMusic;
    public AudioClip gameplayMusic;
    public AudioClip pauseMusic;
    public AudioClip gameOverMusic;

    private float musicVolume = 1f;
    private float musicFadeMultiplier = 1f;

    public AudioLowPassFilter gameplayFilter;

    private Coroutine fadeRoutine;

    private const string MUSIC_KEY = "MusicVolume";

    public GameState CurrentState { get; private set; } = GameState.Gameplay;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            musicSource = GetComponent<AudioSource>();
            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    public void Start()
    {
        ApplyMusicVolume();
        SetGameState(GameState.Gameplay);
    }
    public float MusicVolume
    {
        get { return musicVolume; }
        set { musicVolume = Mathf.Clamp01(value); }
    }


    // ------------------------
    // Music Control
    // ------------------------

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        if (musicSource == null || clip == null) return;

        //  Prevent the song from resetting
        if (musicSource.clip == clip && musicSource.isPlaying) return;

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
    // Volume Control
    // ------------------------

    public void SetMusicVolume(float value)
    {
        musicVolume = Mathf.Clamp01(value);
        ApplyMusicVolume();
    }

    private void ApplyMusicVolume()
    {
        if (musicSource != null)
            musicSource.volume = musicVolume * musicFadeMultiplier;
    }

    // ------------------------
    // Game State Management
    // ------------------------
    public void SetGameState(GameState state)
    {
        CurrentState = state;  // Set the CurrentState to the new state
        Debug.Log("Music state set to: " + state);
        switch (state)
        {
            case GameState.MainMenu:
                musicFadeMultiplier = mainmenuMultiplier;
                PlayMusic(mainmenuMusic); // Play main menu music explicitly
                break;
            case GameState.Gameplay:
                musicFadeMultiplier = gameplayMultiplier;
                PlayMusic(gameplayMusic);  // Play gameplay music when in gameplay state
                break;
            case GameState.Pause:
                musicFadeMultiplier = pauseMultiplier;
                PlayMusic(pauseMusic);  // Play pause music when in pause state
                break;
            case GameState.GameOver:
                musicSource.Stop();
                musicFadeMultiplier = gameOverMultiplier;
                PlayMusic(gameOverMusic, false);  // Play game over music when in game over state
                break;
            default:
                musicFadeMultiplier = defaultMusicMultiplier;
                PlayMusic(mainmenuMusic);  // Default to main menu music if something doesn't match up
                break;
        }

        ApplyMusicVolume();
    }

    public enum GameState
    {
        MainMenu,
        Gameplay,
        Pause,
        GameOver
    }

    // ------------------------
    // Save / Load
    // ------------------------

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MUSIC_KEY, musicVolume);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 0.3f);
    }

    // ------------------------
    // Fade System (Optional)
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

    public void SetStress(float t)
    {
        // volume
        float targetVolume = Mathf.Lerp(1f, 0.4f, t);
        musicSource.volume = musicVolume * targetVolume * musicFadeMultiplier;

        // low-pass (if filter is on SAME source)
        if (gameplayFilter != null)
        {
            gameplayFilter.cutoffFrequency =
                Mathf.Lerp(22000f, 500f, t);
        }
    }



}
