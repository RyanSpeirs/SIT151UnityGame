using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class ShipGameMode : MonoBehaviour
{
    public PlayerShipController player;

    [Header("Level Setup")]
    public string levelName = "Level1";
    
    // This is a flag that indicates if the game is over
    public bool gameOver = false;

    //  Handles for the Health UI
    public TextMeshProUGUI healthDisplay;
    public Slider healthBar;

   

    public AudioClip gameOverMusic;

    public int itemsCollected = 0;

    public AudioSource heartbeatSource;
    public AudioClip heartbeatClip;
    public float heartbeatThreshold = 30f;
    private float heartbeatMaxVolume = 1f;

    public AudioLowPassFilter musicLowPassFilter;  // For applying the muffling effect to game music
    private float normalLowPassFrequency = 22000f;  // Normal frequency of game music
    private float muffledLowPassFrequency = 500f;

    [Header("Game Over UI")]
    public GameObject gameOverMenu;
    public GameObject gameOverDisplay;
    public CanvasGroup gameOverCanvasGroup;

    [Header("Fade Settings")]
    public float uiFadeDuration = 1f;


    // Start is called before the first frame update
    void Start()
    {
        InitialiseLevel();
        heartbeatSource.volume= 0f;
        heartbeatSource.loop = true;
        heartbeatSource.clip = heartbeatClip;

        SetState(GameState.Gameplay);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("ShipGameMode Update running");

        //  Updates both UI Health indicators
        healthDisplay.text = "Health: " + player.health + "%";
        healthBar.value = player.health;

       if (!gameOver && player.health <= 0.01f)
       {
            Debug.Log("GAME OVER TRIGGER CONDITION MET: " + player.health);
            player.health = 0;
            TriggerGameOver();
       }

        if (Input.GetKeyDown(KeyCode.G))
        {
            TriggerGameOver();
        }

        Debug.Log(player.name + " HP: " + player.health);

        if (InputManager.Instance.PausePressed)
        {
            if (CurrentState == GameState.Pause)
                ResumeGame();
            else
                PauseGame();
        }

    }

    private void InitialiseLevel()
    {
        // 1. Tell MusicManager what level we're in
        MusicManager.Instance.SetLevel(levelName);

        // 2. Start gameplay music/state
        SetState(GameState.Gameplay);
    }

    private void HandleHeartbeatSoundAndMusicEffects()
    {
        // If health is below 30%, start playing the heartbeat sound and reduce music volume
        if (player.health < 30f)
        {
            // Trigger heartbeat sound if it's not already playing
            if (!heartbeatSource.isPlaying) // Only start if it's not already playing
            {
                AudioManager.Instance.PlayHeartbeatSound();
            }
            // Apply muffling effect to music
           
        }
        else
        {
            // Stop heartbeat sound if health is above threshold
            AudioManager.Instance.StopHeartbeatSound();
            // Remove muffling effect from music
            
        }
    }



   // public void TriggerPause()
   // {
   //     
   // }



    public void TriggerGameOver()
    {
    
        if (CurrentState == GameState.GameOver) return;  // hard guard

        SetState(GameState.GameOver);

        gameOverDisplay.gameObject.SetActive(true);

        StartCoroutine(GameOverSequence());
    }

    public void PauseGame()
    {
        SetState(GameState.Pause);
    }

    public void ResumeGame()
    {
        SetState(GameState.Gameplay);
    }


    public Image fadeImage;
    public float fadeDuration = 1f;

    IEnumerator FadeToBlack()
    {
        float t = 0f;

        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;

            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = c;

            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    IEnumerator FadeInUI(CanvasGroup cg)
    {
        float t = 0f;

        cg.gameObject.SetActive(true);

        cg.alpha = 0f;
        cg.interactable = false;
        cg.blocksRaycasts = false;

        while (t < uiFadeDuration)
        {
            t += Time.unscaledDeltaTime;

            float progress = t / uiFadeDuration;
            cg.alpha = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        cg.alpha = 1f;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    IEnumerator GameOverSequence()
    {
        gameOver = true;

        // 1. Switch music state to Game Over (stop gameplay + play game over track) superceded by new design
       

        // 2. Fade screen to black (gameplay layer transition)
        yield return StartCoroutine(FadeToBlack());

        // 3. Small pause for impact / readability
        yield return new WaitForSecondsRealtime(0.25f);

        // 4. Enable Game Over UI panel and reset its state BEFORE fading in
        gameOverCanvasGroup.gameObject.SetActive(true);
        gameOverCanvasGroup.alpha = 0f;
        gameOverCanvasGroup.interactable = false;
        gameOverCanvasGroup.blocksRaycasts = false;

        // 5. Fade in Game Over UI
        yield return StartCoroutine(FadeInUI(gameOverCanvasGroup));

        // 6. Wait for Game Over music to finish (optional cinematic pacing)
        yield return new WaitUntil(() =>
            !MusicManager.Instance.IsSecondaryMusicPlaying());
    }


    public GameState CurrentState { get; private set; }

    public void SetState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;

        Debug.Log("State changed to: " + newState);

        MusicManager.Instance.ApplyState(newState);
    }

    
}

