using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnDistanceFromZero;
    [SerializeField] private float originDistanceFromZero;

    [SerializeField] private GameObjectCollection enemyCollection;
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private float timeBetweenSpawns;

    [SerializeField] private List<int> waves;
    [SerializeField] private float timeBetweenWaves;

    // state
    private float _spawnTimer;
    private int _currentWave;
    
    // Start is called before the first frame update
    void Start() {
        _currentWave = 0;
        if(waves.Count >= 1) SpawnWave(_currentWave);
    }

    // Update is called once per frame
    void Update() {
        UpdateRandomSpawning();
        /*if (_currentWave < waves.Count) {
            UpdateWaveSpawning();
        }
        else {
            UpdateRandomSpawning();
        }*/
    }

    private void UpdateWaveSpawning() {
        if (enemyCollection.Count == 0) {
            var nextWave = _currentWave + 1;
            if (nextWave < waves.Count) {
                SpawnWave(nextWave);
                _currentWave = nextWave;
            }
        }
    }
    
    private void SpawnWave(int waveIndex) {
        for (int i = 0; i < waves[waveIndex]; i++) {
            SpawnEnemy();
        }
    }

    private void UpdateRandomSpawning() {
        if (_spawnTimer > 0) _spawnTimer -= Time.deltaTime;
        else if(enemyCollection.Count < maxEnemyCount) {
            SpawnEnemy();
            _spawnTimer = timeBetweenSpawns;
        }
    }

    private void SpawnEnemy() {
        // pick random position that's offscreen 
        var randomUnitSphere = Random.onUnitSphere;
        Vector2 randomDirection = new Vector2(randomUnitSphere.x, randomUnitSphere.y);
        randomDirection.Normalize();

        var originPosition = randomDirection * originDistanceFromZero;
        var randomPosition = randomDirection * spawnDistanceFromZero;

        // instantiate prefab at position
        var enemyMovement = Instantiate(enemyPrefab, randomPosition, Quaternion.identity).GetComponent<EnemyMovement>();
        enemyMovement.SetOrigin(originPosition);
        
        Debug.Log("spawned enemy");
    }
}