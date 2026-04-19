using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class ShipGameMode : MonoBehaviour
{
    public PlayerShipController player;
    
    // This is a flag that indicates if the game is over
    public bool gameOver = false;
    public TextMeshProUGUI gameOverDisplay;
    //  Handles for the Health UI
    public TextMeshProUGUI healthDisplay;
    public Slider healthBar;

    
    public AudioClip gameOverFlourish;
    public bool hasPlayedGameOverSound = false;

   

    public int itemsCollected = 0;

    public AudioSource heartbeatSource;
    public AudioClip heartbeatClip;
    public float heartbeatThreshold = 30f;
    private float heartbeatMaxVolume = 1f;

    public AudioLowPassFilter musicLowPassFilter;  // For applying the muffling effect to game music
    private float normalLowPassFrequency = 22000f;  // Normal frequency of game music
    private float muffledLowPassFrequency = 500f;

    // Start is called before the first frame update
    void Start()
    {
        heartbeatSource.volume= 0f;
        heartbeatSource.loop = true;
        heartbeatSource.clip = heartbeatClip;
    }

    // Update is called once per frame
    void Update()
    {
        //  Updates both UI Health indicators
        healthDisplay.text = "Health: " + player.health + "%";
        healthBar.value = player.health;

        // If the Game Over flag flips true, trigger the Text display
        gameOverDisplay.gameObject.SetActive(gameOver);

        if (gameOver && !hasPlayedGameOverSound)
        {
            // Stop the gameplay music (handled by MusicManager)
            MusicManager.Instance.PlayGameOverMusic();

            // Optionally, play any additional game over flourish sound
            if (gameOverFlourish != null)
            {
                AudioManager.Instance.PlaySFX(gameOverFlourish);
            }

            hasPlayedGameOverSound = true;

            HandleHeartbeatSoundAndMusicEffects();
        }

            
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
            MusicManager.Instance.ApplyMufflingEffect(0.2f); // Reduce music volume and apply effect
        }
        else
        {
            // Stop heartbeat sound if health is above threshold
            AudioManager.Instance.StopHeartbeatSound();
            // Remove muffling effect from music
            MusicManager.Instance.RemoveMufflingEffect();
        }
    }
}

