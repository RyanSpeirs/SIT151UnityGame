using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBase : MonoBehaviour
{


    [Header("Movement")]
    public float speed = 2f;

    [Header("Death")]
    public RuntimeAnimatorController explosion;
    public float deathDelay = 2f;

    [Header("Cleanup")]
    [SerializeField]
    private bool destroyWhenOffscreen = true;

    [Header("Health")]
    public int maxHealth = 1;

    protected int currentHealth;

    protected bool hasBeenVisible = false;
    protected bool isDying = false;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        Move();
        CheckOffscreenDestroy();
    }

    //  This is to ensure an enemy has a default behaviour pattern
    protected virtual void Move()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    //  the collision detection logic
    protected virtual void OnCollisionEnter(Collision collisionInfo)
    {
        if (isDying)
            return;

        Debug.Log(
            "Hit: " +
            gameObject.name +
            " was hit by " +
            collisionInfo.gameObject.name
        );

        if (collisionInfo.gameObject.CompareTag("Bullet"))
        {
            Destroy(collisionInfo.gameObject);
            TakeDamage(1);
        }
    }

    public virtual void Die()
    {
        if (isDying) 
            return;
        
        isDying = true;
        StartCoroutine(DeathRoutine());
    }

    protected virtual IEnumerator DeathRoutine()
    {
        Collider collider = GetComponent<Collider>();

        if (collider != null)
            collider.enabled = false;

        Animator animator = GetComponent<Animator>();

        if (animator != null && explosion != null)
        {
            animator.runtimeAnimatorController = explosion;
        }

        yield return new WaitForSecondsRealtime(deathDelay);

        Destroy(gameObject);
    }

    //  tells us that enemy has been onscreen
    private void OnBecameVisible()
    {
        hasBeenVisible = true;
    }

    private void CheckOffscreenDestroy()
    {
        if (!destroyWhenOffscreen)
            return;

        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null &&
            hasBeenVisible &&
            !renderer.isVisible)
        {
            Destroy(gameObject);
        }
    }

    //  For damage to apply to multi-health enemies
    protected virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

}