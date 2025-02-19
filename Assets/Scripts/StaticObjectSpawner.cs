using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectSpawner : MonoBehaviour
{
    [SerializeField] private WaveSet waveSet;
    [Tooltip("Represents the amount of vertical padding on top and bottom of the screen")]
    [SerializeField] private float verticalSpawnPadding = 5f;
    [SerializeField] private float defaultStaticObjectSpawnFrequency = 3f;
    [SerializeField] private float defaultSpawnTimerVariance = 0.5f;

    private float spawnTimer = 0f;

    private void Start()
    {
        ResetSpawnTimer();
    }
    void Update()
    {
        if (EnemySpawner.Instance.CurrentWave < 0 || !waveSet.waves[EnemySpawner.Instance.CurrentWave].hasStaticObjectObstacles) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnObject();
            ResetSpawnTimer();
        }
    }

    private void SpawnObject()
    {
        Wave wave = waveSet.waves[EnemySpawner.Instance.CurrentWave];
        int randomIndex = Random.Range(0, wave.staticObjectPrefabs.Count);
        GameObject staticObject = wave.staticObjectPrefabs[randomIndex];

        Vector3 screenRightBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, Camera.main.nearClipPlane));
        float verticalSpawnPosition = Random.Range(-verticalSpawnPadding, verticalSpawnPadding);
        Vector3 spawnPosition = new(screenRightBounds.x + 1, verticalSpawnPosition, 0);

        Instantiate(staticObject, spawnPosition, Quaternion.identity);
    }

    private void ResetSpawnTimer()
    {
        int wave = EnemySpawner.Instance.CurrentWave;
        if (wave < 0) wave = 0;
        float spawnFrequency = waveSet.waves[wave].staticObjectSpawnFrequency;
        if (spawnFrequency <= 0) spawnFrequency = defaultStaticObjectSpawnFrequency;
        float spawnTimerVariance = waveSet.waves[wave].spawnTimerVariance;
        if (spawnTimerVariance <= 0) spawnTimerVariance = defaultSpawnTimerVariance;

        spawnTimer = spawnFrequency
            + Random.Range(-spawnTimerVariance, spawnTimerVariance);
    }
}
