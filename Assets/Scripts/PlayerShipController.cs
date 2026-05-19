using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public GameObject bulletTemplate;
    public float health = 100.0f;
    public ShipGameMode gameMode;

    public AudioClip shootClip;

    private Camera mainCamera;

    [SerializeField]
    private float screenPadding = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         // spawns bullets when pressedw
         if (Input.GetKeyDown(KeyCode.Space))
         {
             if (!gameMode.gameOver) //  stop bullets spawning when the ship is destroyed
             {
                 GameObject bullet = Instantiate(bulletTemplate, transform.position + new Vector3(0.0f, 0.6f, 0.0f), transform.rotation);
                 GetComponent<AudioSource>().Play();
             }
         }

         // directional movement, hard-coded to the WASD
         if (Input.GetKey(KeyCode.A))
          {
              transform.position += new Vector3(-0.01f, 0.0f, 0.0f);
          }

          if (Input.GetKey(KeyCode.D))
          {
              transform.position += new Vector3(+0.01f, 0.0f, 0.0f);
          }

          if (Input.GetKey(KeyCode.W))
          {
              transform.position += new Vector3(0.00f, +0.01f, 0.0f);
          }

          if (Input.GetKey(KeyCode.S))
          {
              transform.position += new Vector3(0.00f, -0.01f, 0.0f);
          }  */

        if (gameMode != null && gameMode.CurrentState == GameState.GameOver)
            return;

        // --- FIRE ---
        if (InputManager.Instance != null && InputManager.Instance.FirePressed)
        {
            Instantiate(
                bulletTemplate,
                transform.position + new Vector3(0.0f, 0.6f, 0.0f),
                transform.rotation
            );

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(shootClip);
            }

            AudioManager.Instance.PlaySFX(shootClip);
        }

        // --- MOVEMENT ---
        if (InputManager.Instance == null) return;
        Vector2 input = InputManager.Instance.MoveInput;

        float speed = 5f; // tweak this instead of hardcoding 0.01
        Vector3 direction = new Vector3(input.x, input.y, 0f);
        transform.position += direction * speed * Time.deltaTime;

        // --- CLAMP TO CAMERA ---

        Vector3 pos = transform.position;

        // Camera world bounds
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Clamp player inside visible area
        pos.x = Mathf.Clamp(
            pos.x,
            mainCamera.transform.position.x - camWidth + screenPadding,
            mainCamera.transform.position.x + camWidth - screenPadding
        );

        pos.y = Mathf.Clamp(
            pos.y,
            mainCamera.transform.position.y - camHeight + screenPadding,
            mainCamera.transform.position.y + camHeight - screenPadding
        );

        transform.position = pos;

    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("EnemyShip")) 
        { 
            health = health - 05.0f; // updates health
            Debug.Log("Current Health: " + health + ". Collision with: " + collisionInfo.gameObject.name);
            EnemyBase enemy = collisionInfo.gameObject.GetComponent<EnemyBase>(); // gets the collision info from the enemy base class
            if (enemy != null)
            {
                enemy.Die(); // tells objects with EnemyBase to die
            } 

            if (health <= 0.0f)
            {
                gameMode.TriggerGameOver(); // switch game over flag to true

                GetComponent<Collider>().enabled = false;

                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
    
}
