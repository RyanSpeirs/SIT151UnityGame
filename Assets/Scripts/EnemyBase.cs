using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float speed = 2f;
    public RuntimeAnimatorController explosion;
    public float deathDelay = 2f;

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    protected virtual void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.CompareTag("Bullet"))
        {
            Destroy(collisionInfo.gameObject);
            Die();
        }
    }

    protected void Die()
    {
        StartCoroutine(DeathRoutine());
    }

    IEnumerator DeathRoutine()
    {
        GetComponent<Collider>().enabled = false;

        if (explosion != null)
            GetComponent<Animator>().runtimeAnimatorController = explosion;

        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject);
    }
}