using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObjectCollection enemyCollection;
    
    [Header("Spawning")]
    [SerializeField] private Vector2 originRange;
    private enum SpawningMode {
        Wave,
        Random,
    };
    [SerializeField] private SpawningMode spawningMode;
    [SerializeField] private bool spawnRandomAfterLastWave;

    [Header("Wave Spawning")]
    [SerializeField] private List<int> waves;
    [SerializeField] private float timeBetweenWaves;

    [Header("Random Spawning")]
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private float timeBetweenRandomSpawn;
    

    // state
    private enum WaveState { ReadyForWave, ActiveWave, DelayWave }
    private WaveState _waveState;
    private float _waveDelayTimer;
    private int _currentWave;
    private float _randomSpawnTimer;
    
    // Start is called before the first frame update
    void Start() {
        _currentWave = 0;
    }

    // Update is called once per frame
    void Update() {
        if (spawningMode == SpawningMode.Random) {
            UpdateRandomSpawning();
        }
        else if (spawningMode == SpawningMode.Wave) {
            UpdateWaveSpawning();
        }
    }
    
    private void UpdateRandomSpawning() {
        if (_randomSpawnTimer > 0) _randomSpawnTimer -= Time.deltaTime;
        else if(enemyCollection.Count < maxEnemyCount) {
            SpawnEnemy();
            _randomSpawnTimer = timeBetweenRandomSpawn;
        }
    }

    private void UpdateWaveSpawning() {
        // if ready, spawn enemies from currentwave and set state to active wave
        if (_waveState == WaveState.ReadyForWave) {
            SpawnWave(_currentWave);
            _waveState = WaveState.ActiveWave;
            ToastUI.Instance.QueueToast("Wave " + _currentWave);
        }
        // if active wave, wait for all enemies to die, then set state to delay 
        else if (_waveState == WaveState.ActiveWave) {
            if (enemyCollection.Count == 0) {
                _waveState = WaveState.DelayWave;
                _waveDelayTimer = timeBetweenWaves;
            }
        }
        // if delay, wait for time, then currentwave++. if valid, then set state to ready
        else if (_waveState == WaveState.DelayWave) {
            if (_waveDelayTimer <= 0) {
                // if there is no next wave 
                if (_currentWave + 1 >= waves.Count) {
                    // either transition to random spawning
                    if (spawnRandomAfterLastWave) {
                        spawningMode = SpawningMode.Random;
                    }
                    // or replay the last wave
                    else {
                        _waveState = WaveState.ReadyForWave;
                    }
                }
                // advance to next wave
                else {
                    _currentWave++;
                    _waveState = WaveState.ReadyForWave;
                }
            }
            else _waveDelayTimer -= Time.deltaTime;
        }
    }
    
    private void SpawnWave(int waveIndex) {
        for (int i = 0; i < waves[waveIndex]; i++) {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy() {
        // pick origin position that's onscreen
        var originPosition = new Vector3(Random.Range(-originRange.x, originRange.x), Random.Range(-originRange.y, originRange.y), 0);
        var spawnPosition = Random.onUnitSphere * (originRange.x * 1.5f);

        // instantiate prefab at position
        var enemyMovement = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity).GetComponent<EnemyMovement>();
        enemyMovement.SetAnchor(originPosition);
        
        //Debug.Log("spawned enemy");
    }
}
