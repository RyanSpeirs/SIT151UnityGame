using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBasic : EnemyBase
{   
    public RuntimeAnimatorController explosion;

    //  because we're using DeltaTime we need to change how enemy movement works so it pauses etc
    public float speed = 0.5f;
    public Vector3 direction = Vector3.down;

    // Update is called once per frame
    void Update()
    {
        // makes the enemy ships fall downwards
        transform.position += direction * speed * Time.deltaTime;

        // if they fall off the screen, they are destroyed
        if (transform.position.y < -5.0f )
        {
            Destroy(this.gameObject);
        }

        //  Debug.Log(Time.timeScale);

    }

    // this is basically logging for debugging collisions
    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("Hit: " + this.gameObject.name + " was hit by " + collisionInfo.gameObject.name);

        // destroys the bullet when it hits the ship
        if (collisionInfo.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(destroyActor(collisionInfo.gameObject));
        }
    }

    //  if the ship gets shot by a bullet 
   public IEnumerator destroyActor (GameObject bullet)
   {
        if (bullet != null)
        {
            Destroy(bullet);
        }
        // removes the collider so it doesn't exist as a physical object
        GetComponent<Collider>().enabled = false;
        // calls the sprite flipbook for the explosion
        GetComponent<Animator>().runtimeAnimatorController = explosion; 
        //  waits for a moment before destroying it so the animation can play out
        yield return new WaitForSecondsRealtime(2.0f);
        Destroy(gameObject);
   }
    


}
