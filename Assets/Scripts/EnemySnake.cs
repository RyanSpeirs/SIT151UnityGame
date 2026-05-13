using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : EnemyBase
{
    public float waveAmplitude = 2f;
    public float waveFrequency = 3f;
    public float forwardSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        
        startPos = transform.position;
    }

    protected override void Move()
    {
        float x = Mathf.Sin(Time.time * waveFrequency) * waveAmplitude;

        transform.position += Vector3.down * forwardSpeed * Time.deltaTime;

        transform.position = new Vector3(
            startPos.x + x,
            transform.position.y,
            transform.position.z
        );
    }
}