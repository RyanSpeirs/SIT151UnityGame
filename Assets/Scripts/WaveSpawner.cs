using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveSpawner : MonoBehaviour
{
    [Header("Enemy Types")]
    public List<GameObject> enemyPrefabs;

    [Header("Wave Settings")]
    public List<Wave> waves = new List<Wave>();
    public float timeBetweenWaves = 3f;

    [Header("Spawn Area")]
    public float spawnXRange = 6f;
    public float spawnY = 6f;

    private int currentWaveIndex = 0;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));

            currentWaveIndex++;

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(wave.spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0) return;

        float x = Random.Range(-spawnXRange, spawnXRange);
        Vector3 pos = new Vector3(x, spawnY, 0f);

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];

        Instantiate(prefab, pos, Quaternion.identity);
    }
}

[System.Serializable]
public class Wave
{
    public int enemyCount = 5;
    public float spawnDelay = 0.3f;
}