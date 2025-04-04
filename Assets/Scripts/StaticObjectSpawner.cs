using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectSpawner : MonoBehaviour
{
    [SerializeField] private MissionState missionState;
    [SerializeField] private FloatGameEvent displayWarningSymbol;
    [SerializeField] private WaveSet waveSet;
    [Tooltip("Represents the amount of vertical padding on top and bottom of the screen")]
    [SerializeField] private float verticalSpawnPadding = 5f;
    [SerializeField] private float defaultStaticObjectSpawnFrequency = 3f;
    [SerializeField] private float defaultSpawnTimerVariance = 0.5f;
    [Tooltip("Represents the amount of time the warning symbol will be displayed before the object is spawned")]
    [SerializeField] private float waitTimeBeforeSpawn = 2f;
    [SerializeField] private BoolVariable isReadyToSpawnStaticObject;

    private float spawnTimer = 0f;

    private void Start()
    {
        ResetSpawnTimer();
        isReadyToSpawnStaticObject.Value = false;

        if (missionState.CurrentScene != null && missionState.CurrentScene is LevelScene levelScene){
            waveSet = levelScene.waveSet;
        }
    }
    void Update()
    {
        if (EnemySpawner.Instance.CurrentWave < 0 || !waveSet.waves[EnemySpawner.Instance.CurrentWave].hasStaticObjectObstacles) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            StartCoroutine(SpawnObject());
            ResetSpawnTimer();
        }
    }

    private IEnumerator SpawnObject()
    {
        Wave wave = waveSet.waves[EnemySpawner.Instance.CurrentWave];
        int randomIndex = Random.Range(0, wave.staticObjectPrefabs.Count);
        GameObject staticObject = wave.staticObjectPrefabs[randomIndex];

        Vector3 screenRightBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 0.5f, Camera.main.nearClipPlane));
        float verticalSpawnPosition = Random.Range(-verticalSpawnPadding, verticalSpawnPadding);
        Vector3 spawnPosition = new(screenRightBounds.x + 1, verticalSpawnPosition, 0);

        displayWarningSymbol.Raise(spawnPosition.y);

        yield return new WaitUntil(()=> isReadyToSpawnStaticObject.Value == true);
        Instantiate(staticObject, spawnPosition, Quaternion.identity);
        isReadyToSpawnStaticObject.Value = false;
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
