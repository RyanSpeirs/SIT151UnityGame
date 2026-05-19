using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnakeSegment : MonoBehaviour
{
    [Header("Health")]
    public int maxHP = 3;

    private int currentHP;

    [Header("Snake Data")]
    public int segmentIndex;
    public EnemySnake owner;

    [Header("Visuals")]
    public SpriteRenderer mainRenderer;
    public SpriteRenderer outlineRenderer;

    public float flashDuration = 0.1f;

    private bool isDead = false;

    private Color originalColor;

    void Start()
    {
        currentHP = maxHP;

        if (mainRenderer != null)
        {
            originalColor = mainRenderer.color;
        }

        if (outlineRenderer != null)
        {
            outlineRenderer.enabled = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHP -= damage;

        // Still alive
        if (currentHP > 0)
        {
            StartCoroutine(DamageFlash());
        }
        else
        {
            isDead = true;

            if (owner != null)
            {
                owner.DestroyFromIndex(segmentIndex);
            }
        }
    }

    private IEnumerator DamageFlash()
    {
        // White flash
        if (mainRenderer != null)
        {
            mainRenderer.color = Color.white;
        }

        // Yellow outline
        if (outlineRenderer != null)
        {
            outlineRenderer.enabled = true;
            outlineRenderer.color = Color.yellow;
        }

        yield return new WaitForSeconds(flashDuration);

        // Restore
        if (mainRenderer != null)
        {
            mainRenderer.color = originalColor;
        }

        if (outlineRenderer != null)
        {
            outlineRenderer.enabled = false;
        }
    }
}