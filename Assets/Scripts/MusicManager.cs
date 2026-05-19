using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header ("Music Sources")]
    public AudioSource gameplaySource;
    public AudioSource secondarySource;


    [Header("Level Music Database")]
    public List<LevelAudioProfile> levelProfiles;


    [Header("Track Balance")]
    public float defaultMusicMultiplier = 1f;
    public float gameplayMultiplier = 0.8f;
    public float pauseMultiplier = 1f;
    public float gameOverMultiplier = 1.2f;
    public float mainmenuMultiplier = 1.0f;

    private bool gameplayWasPaused;
    private bool hasInitialisedState = false;

    public AudioClip mainmenuMusic;
    public AudioClip gameplayMusic;
    public AudioClip pauseMusic;
    public AudioClip gameOverMusic;

    private float musicVolume = 1f;
    private float musicFadeMultiplier = 1f;
    private AudioClip lastOneShotClip;
    
    public AudioLowPassFilter gameplayFilter;

    private Coroutine fadeRoutine;

    private const string MUSIC_KEY = "MusicVolume";

    public GameState CurrentState { get; private set; }
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


    public void Start()
    {
        ApplyMusicVolume();
       
    }
    public float MusicVolume
    {
        get { return musicVolume; }
        set { musicVolume = Mathf.Clamp01(value); }
    }


    // ------------------------
    // Music Control
    // ------------------------

    public void PlayGameplayMusic(AudioClip clip, bool loop = true)
    {
        if (gameplaySource == null || clip == null) return;

        gameplaySource.loop = loop;

        // If switching track
        if (gameplaySource.clip != clip)
        {
            gameplaySource.clip = clip;
            gameplayWasPaused = false;
            gameplaySource.Play();
            ApplyMusicVolume();
            return;
        }

        // If resuming from pause
        if (gameplayWasPaused)
        {
            gameplayWasPaused = false;
            gameplaySource.UnPause();
            return;
        }

        // Already playing
        if (gameplaySource.isPlaying)
            return;

        // Fresh start fallback
        gameplaySource.Play();
        ApplyMusicVolume();
    }

    public void PlaySecondaryMusic(AudioClip clip, bool loop = true)
    {
        if (secondarySource == null || clip == null) return;

       
        if (secondarySource.clip == clip && secondarySource.isPlaying)
            return;

        secondarySource.clip = clip;
        secondarySource.loop = loop;

        if (!loop)
            lastOneShotClip = clip;

        ApplyMusicVolume();
        secondarySource.Play();
    }

    public bool IsSecondaryMusicPlaying()
    {
        return secondarySource != null && secondarySource.isPlaying;
    }

    private void StopAllMusic()
    {
        gameplaySource.Stop();
        secondarySource.Stop();
    }

    public void PauseMusic()
    {
        if (gameplaySource.isPlaying)
        {
            gameplaySource.Pause();
            gameplayWasPaused = true;
        }
    }

    public void ResumeMusic()
    {
        if (gameplaySource != null)
        {
            gameplaySource.UnPause();
        }
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
        if (gameplaySource != null)
        {
            gameplaySource.volume =
                musicVolume *
                musicFadeMultiplier;
        }

        if (secondarySource != null)
        {
            secondarySource.volume =
                musicVolume *
                musicFadeMultiplier;
        }

        Debug.Log("Secondary volume: " + secondarySource.volume);
    }

    // ------------------------
    // Game State Management
    // ------------------------
    public void ApplyState(GameState state)
    {

        if (hasInitialisedState && CurrentState == state)
            return;

        hasInitialisedState = true;
        CurrentState = state;


        switch (state)
        {
            case GameState.MainMenu:
                PlaySecondaryMusic(mainmenuMusic);
                break;

            case GameState.Gameplay:
                {

                    secondarySource.Stop();
                    
                  
                    if (gameplayWasPaused)
                    {
                        gameplayWasPaused = false;
                        gameplaySource.UnPause();
                    }
                    else
                    {
                        if (!gameplaySource.isPlaying)
                        {
                            gameplaySource.clip = gameplayMusic;
                            gameplaySource.loop = true;
                            gameplaySource.Play();
                        }
                    }

                    if (gameplayFilter != null)
                    {
                        gameplayFilter.enabled = false;
                    }
                    break;
                }

            case GameState.Pause:
                {
                    Debug.Log("PAUSE MUSIC START");

                    // Pause gameplay once
                    if (gameplaySource.isPlaying)
                    {
                        gameplaySource.Pause();
                        gameplayWasPaused = true;
                    }

                    // ALWAYS ensure pause music plays correctly
                    if (secondarySource.clip != pauseMusic)
                    {
                        secondarySource.Stop();
                        secondarySource.clip = pauseMusic;
                        secondarySource.loop = true;
                        secondarySource.Play();
                    }
                    else
                    {
                        // If it's not playing for ANY reason, restart it
                        if (!secondarySource.isPlaying)
                        {
                            secondarySource.time = 0f; // optional reset for consistency
                            secondarySource.Play();
                        }
                    }

                    gameplayFilter.enabled = false;
                    break;
                }

            case GameState.GameOver:
                StopAllMusic();
                PlaySecondaryMusic(gameOverMusic, false);
                break;
        }

        ApplyMusicVolume();
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
    // Fade System (optional, may be implemented later)
    // ------------------------

    public void FadeMusic(float targetMultiplier, float duration)
    {
        if (gameplaySource == null || secondarySource == null) return;

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

    //-----------------------------------
    //      Stress (Heartbeat effect)
    //-----------------------------------

    public void SetStress(float t)
    {
        if (CurrentState != GameState.Gameplay)
            return;

        float targetVolume =
            Mathf.Lerp(1f, 0.4f, t);

        gameplaySource.volume =
            musicVolume *
            targetVolume *
            musicFadeMultiplier;

        if (gameplayFilter != null)
        {
            gameplayFilter.enabled = true;

            gameplayFilter.cutoffFrequency =
                Mathf.Lerp(22000f, 500f, t);
        }
    }

    //---------------------------------------------------
    //      Level Selection (defines gameplay track based on the level active)
    //---------------------------------------------------
    public void SetLevel(string levelName)
    {
        var profile = levelProfiles.Find(p => p.levelName == levelName);

        if (profile != null)
        {
            gameplayMusic = profile.gameplayMusic;
        }
    }

    public void ResetAudioState()
    {
        gameplaySource.Stop();
        secondarySource.Stop();
        lastOneShotClip = null;
        gameplayWasPaused = false;
        musicFadeMultiplier = 1f;

        if (gameplayFilter != null)
            gameplayFilter.cutoffFrequency = 22000f;
    }

    public void StopAllSecondaryMusic()
    {
        if (secondarySource == null) return;

        secondarySource.Stop();
        secondarySource.clip = null;
    }

   
}
