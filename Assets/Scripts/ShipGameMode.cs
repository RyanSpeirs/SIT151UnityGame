using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShipGameMode : MonoBehaviour
{
    public PlayerShipController player;
    
    // This is a flag that indicates if the game is over
    public bool gameOver = false;
    public TextMeshProUGUI gameOverDisplay;
    //  Handles for the Health UI
    public TextMeshProUGUI healthDisplay;
    public Slider healthBar;

    public AudioSource backgroundAudioSource;
    public AudioClip gameOverFlourish;
    public AudioSource gameOverAudioSource;
    public bool hasPlayedGameOverSound = false;

    public int itemsCollected = 0;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //  Updates both UI Health indicators
        healthDisplay.text = "Health: " + player.health + "%";
        healthBar.value = player.health;

        // if the Game Over flag flips true, trigger the Text display
        gameOverDisplay.gameObject.SetActive(gameOver);

        if (gameOver && !hasPlayedGameOverSound)
        {
            backgroundAudioSource.Stop();
            gameOverAudioSource.PlayOneShot(gameOverFlourish);
            hasPlayedGameOverSound=true;    
        }

    }
}
