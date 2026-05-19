using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStressController : MonoBehaviour
{
    [Header("References")]
    public PlayerShipController player;
    private AudioSource heartbeatSource;
    public AudioClip heartbeatClip;
    public AudioLowPassFilter musicLowPass;
    

    [Header("Heartbeat Settings")]
    public float heartbeatStartHP = 50f;
    public float maxHeartbeatRate = 0.25f; // fastest delay
    public float minHeartbeatRate = 1.2f;   // slowest delay

    [Header("Music Filter Settings")]
    public float filterStartHP = 35f;
    public float normalCutoff = 22000f;
    public float stressedCutoff = 500f;

    [Header("Smoothing")]
    public float smoothingSpeed = 5f;

    [Header("Curves")]
    public AnimationCurve heartbeatCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public AnimationCurve filterCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Coroutine heartbeatRoutine;
    private float heartbeatIntensity;
    private bool heartbeatActive;

    void Update()
    {
        if (player == null) return;
        float hp = player.health;

        UpdateHeartbeat(hp);
        UpdateMusicFilter(hp);
    }

    void Awake()
    {
        Debug.Log("AudioStressController AWAKE on: " + gameObject.name);

        heartbeatSource = gameObject.AddComponent<AudioSource>();
        heartbeatSource.playOnAwake = false;
        heartbeatSource.loop = false;
    }

    // --------------------------
    // HEARTBEAT SYSTEM
    // --------------------------
    void UpdateHeartbeat(float hp)
    {
        if (hp <= heartbeatStartHP)
        {
            if (heartbeatRoutine == null)
            {
                heartbeatRoutine = StartCoroutine(HeartbeatLoop());
                heartbeatActive = true;
            }

            float t = Mathf.InverseLerp(heartbeatStartHP, 0f, hp);
            heartbeatIntensity = heartbeatCurve.Evaluate(t);
        }
        else
        {
            if (heartbeatActive)
            {
                StopHeartbeat();
            }
        }
    }

    void StopHeartbeat()
    {
        heartbeatActive = false;
        heartbeatIntensity = 0f;

        if (heartbeatRoutine != null)
        {
            StopCoroutine(heartbeatRoutine);
            heartbeatRoutine = null;
        }

        if (heartbeatSource != null)
        {
            heartbeatSource.Stop();
        }
    }

    IEnumerator HeartbeatLoop()
    {
        while (true)
        {
            if (heartbeatSource == null || heartbeatClip == null)
                yield break;

            float volume = Mathf.Lerp(0.3f, 1f, heartbeatIntensity);
           

            heartbeatSource.PlayOneShot(heartbeatClip, volume);

            float delay = Mathf.Lerp(minHeartbeatRate, maxHeartbeatRate, heartbeatIntensity);
            yield return new WaitForSeconds(delay);
        }
    }

    // --------------------------
    // MUSIC FILTER SYSTEM
    // --------------------------
    void UpdateMusicFilter(float hp)
    {
        if (musicLowPass == null) return;

        float targetT;

        if (hp <= filterStartHP)
        {
            targetT = Mathf.InverseLerp(filterStartHP, 0f, hp);
            targetT = filterCurve.Evaluate(targetT);
        }
        else
        {
            targetT = 0f;
        }

        float targetCutoff = Mathf.Lerp(normalCutoff, stressedCutoff, targetT);

        musicLowPass.cutoffFrequency = Mathf.Lerp(
            musicLowPass.cutoffFrequency,
            targetCutoff,
            Time.deltaTime * smoothingSpeed
        );
    }

    public void ResetHeartbeat()
    {
        if (heartbeatRoutine != null)
        {
            StopCoroutine(heartbeatRoutine);
            heartbeatRoutine = null;
        }

        heartbeatActive = false;
        heartbeatIntensity = 0f;

        if (heartbeatSource != null)
        {
            heartbeatSource.Stop();
        }
    }

    void OnDisable()
    {
        StopHeartbeat();
    }
}