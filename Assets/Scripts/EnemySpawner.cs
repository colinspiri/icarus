using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector2 originRange;

    [SerializeField] private GameObjectCollection enemyCollection;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private float timeBetweenRandomSpawn;

    [SerializeField] private List<int> waves;
    [SerializeField] private float timeBetweenWaves;

    // state
    private enum SpawningState { Random, ReadyForWave, ActiveWave, DelayWave }
    private SpawningState _spawningState;
    private float _waveDelayTimer;
    private int _currentWave;
    private float _randomSpawnTimer;
    
    // Start is called before the first frame update
    void Start() {
        _currentWave = 0;
        
        // if there are waves, start in wave spawning mode. otherwise start in random spawning
        if (waves.Count >= 1) {
            _spawningState = SpawningState.ReadyForWave;
        }
        else _spawningState = SpawningState.Random;
    }

    // Update is called once per frame
    void Update() {
        // if state is random spawning, do that
        if (_spawningState == SpawningState.Random) {
            UpdateRandomSpawning();
        }
        // if ready, spawn enemies from currentwave and set state to active wave
        else if (_spawningState == SpawningState.ReadyForWave) {
            SpawnWave(_currentWave);
            _spawningState = SpawningState.ActiveWave;
        }
        // if active wave, wait for all enemies to die, then set state to delay 
        else if (_spawningState == SpawningState.ActiveWave) {
            if (enemyCollection.Count == 0) {
                _spawningState = SpawningState.DelayWave;
                _waveDelayTimer = timeBetweenWaves;
            }
        }
        // if delay, wait for time, then currentwave++. if valid, then set state to ready
        else if (_spawningState == SpawningState.DelayWave) {
            if (_waveDelayTimer <= 0) {
                _currentWave++;
                if (_currentWave < waves.Count) {
                    _spawningState = SpawningState.ReadyForWave;
                }
                else _spawningState = SpawningState.Random;
            }
            else _waveDelayTimer -= Time.deltaTime;
        }
    }
    
    private void SpawnWave(int waveIndex) {
        for (int i = 0; i < waves[waveIndex]; i++) {
            SpawnEnemy();
        }
    }

    private void UpdateRandomSpawning() {
        if (_randomSpawnTimer > 0) _randomSpawnTimer -= Time.deltaTime;
        else if(enemyCollection.Count < maxEnemyCount) {
            SpawnEnemy();
            _randomSpawnTimer = timeBetweenRandomSpawn;
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
