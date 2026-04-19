using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class PlayerShipController : MonoBehaviour
{
    public GameObject bulletTemplate;
    public float health = 100.0f;
    public ShipGameMode gameMode;
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        if (gameMode.gameOver) return;

        // --- FIRE ---
        if (InputManager.Instance != null && InputManager.Instance.FirePressed)
        {
            Instantiate(
                bulletTemplate,
                transform.position + new Vector3(0.0f, 0.6f, 0.0f),
                transform.rotation
            );

            audioSource.Play();
        }

        // --- MOVEMENT ---
        if (InputManager.Instance == null) return;
        Vector2 input = InputManager.Instance.MoveInput;

        float speed = 5f; // tweak this instead of hardcoding 0.01
        Vector3 direction = new Vector3(input.x, input.y, 0f);
        transform.position += direction * speed * Time.deltaTime;

    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("EnemyShip")) 
        { 
            health = health - 05.0f; // updates health
            Debug.Log("Current Health: " + health + ". Collision with: " + collisionInfo.gameObject.name);
            EnemyBasic enemy = collisionInfo.gameObject.GetComponent<EnemyBasic>(); // gets the collision info from the enemy ship class
            StartCoroutine(enemy.destroyActor(null)); // blows up the enemy ship

            if (health <= 0.0f)
            {
                gameMode.gameOver = true; // switch game over flag to true

                GetComponent<Collider>().enabled = false;

                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
    
}
