using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour {
    public static EnemySpawner Instance;

    [Header("Components")] 
    [SerializeField] private MissionState missionState;
    [SerializeField] private GameObjectCollection enemyCollection;
    [SerializeField] private GameObject prototypeEnemyPrefab;
    [SerializeField] private GameObject fighter1Prefab;
    [SerializeField] private GameObject fighter2Prefab;
    [SerializeField] private GameObject fighter3Prefab;
    [SerializeField] private GameObject bomberPrefab;
    [SerializeField] private GameObject homingMissilePrefab;
    [SerializeField] private GameObject eliteEnemyPrefab;
    [SerializeField] private GameObject laserEnemyPrefab;
    [SerializeField] private GameObject shotgunEnemyPrefab;

    [Header("Anchor Points")] 
    [SerializeField] private List<Transform> anchorPoints;

    [Header("Spawning Mode")]
    [SerializeField] private SpawningMode spawningMode;
    private enum SpawningMode { Wave, Random, };
    [SerializeField] private bool spawnRandomAfterLastWave;

    [Header("Wave Spawning")]
    [SerializeField] private WaveSet waveSet;
    [SerializeField] private float timeBetweenWaves;

    [Header("Random Spawning")]
    [SerializeField] private int maxEnemyCount;
    [SerializeField] private MinMaxFloat randomSpawnTimeMinMax;
    [SerializeField] private AnimationCurve randomSpawnTimeByEnemyPercent;

    // state 
    private enum WaveState { WaitingToStart, ReadyForWave, ActiveWave, DelayWave, End }
    private WaveState _waveState = WaveState.WaitingToStart;
    private float _waveDelayTimer;
    private int _currentWave;
    private float _randomSpawnTimer;
    
    // events 
    public event Action OnCompleteWaves;

    private void Awake() {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        _currentWave = -1;
        _waveDelayTimer = timeBetweenWaves;
        if (missionState.currentScene != null && missionState.currentScene is LevelScene levelScene) {
            waveSet = levelScene.waveSet;
        }
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
        if (enemyCollection.Count >= maxEnemyCount) return;
        
        _randomSpawnTimer += Time.deltaTime;

        var enemyAlivePercent = (float)enemyCollection.Count / maxEnemyCount;
        var t = randomSpawnTimeByEnemyPercent.Evaluate(enemyAlivePercent);
        var randomSpawnTime = randomSpawnTimeMinMax.LerpValue(t);

        if (_randomSpawnTimer >= randomSpawnTime) {
            List<EnemyType> allEnemyTypes = new List<EnemyType>() { EnemyType.Fighter1 , EnemyType.Fighter2, EnemyType.Fighter3, EnemyType.Bomber, EnemyType.HomingMissile, EnemyType.EliteEnemy, EnemyType.LaserEnemy, EnemyType.ShotgunEnemy};
            var enemyType = allEnemyTypes[Random.Range(0, allEnemyTypes.Count)];
            
            int randomIndex = Random.Range(0, anchorPoints.Count);
            Vector3 anchorPoint = anchorPoints[randomIndex].position;
            
            SpawnEnemy(enemyType, anchorPoint);

            _randomSpawnTimer = 0;
        }
    }

    private void UpdateWaveSpawning() {
        if (_waveState is WaveState.WaitingToStart or WaveState.End) return;
        
        // if ready, spawn enemies from currentwave and set state to active wave
        if (_waveState == WaveState.ReadyForWave) {
            SpawnWave(_currentWave);
            _waveState = WaveState.ActiveWave;
            ToastUI.Instance.QueueToast("Wave " + (_currentWave + 1) + "/" + waveSet.NumWaves);
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
                if (_currentWave + 1 >= waveSet.NumWaves) {
                    OnCompleteWaves?.Invoke();
                    
                    // either transition to random spawning
                    if (spawnRandomAfterLastWave) {
                        spawningMode = SpawningMode.Random;
                    }
                    // or do nothing
                    else {
                        _waveState = WaveState.End;
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

    public void StartWaveSpawning() {
        _waveState = WaveState.DelayWave;
    }
    
    private void SpawnWave(int waveIndex) {
        Wave wave = waveSet.waves[waveIndex];

        List<int> availableAnchorPoints = new List<int>();

        foreach (var numEnemies in wave.enemies) {
            for (int i = 0; i < numEnemies.count; i++) {

                // when all anchor points are used up, fill it back up again
                if (availableAnchorPoints.Count == 0) {
                    for (var index = 0; index < anchorPoints.Count; index++) {
                        availableAnchorPoints.Add(index);
                    }
                }
                
                // pick a random anchor point 
                int randomIndex = Random.Range(0, availableAnchorPoints.Count);
                Vector3 anchorPoint = anchorPoints[availableAnchorPoints[randomIndex]].position;
                availableAnchorPoints.RemoveAt(randomIndex);

                // spawn enemy
                SpawnEnemy(numEnemies.enemyType, anchorPoint);
            }
        }
    }

    private void SpawnEnemy(EnemyType enemyType, Vector3 anchorPoint) {
        // get prefab from enemy type
        var enemyPF = GetEnemyPrefab(enemyType);
        
        // pick a spawn position 
        Vector3 directionOutsideAnchor = anchorPoint.normalized;
        directionOutsideAnchor.z = 0;
        var spawnPosition = anchorPoint + 3 * directionOutsideAnchor;

        // instantiate
        if (enemyPF.GetComponent<EnemyMovement>() != null)
        {
            var enemyMovement = Instantiate(enemyPF, spawnPosition, Quaternion.identity).GetComponent<EnemyMovement>();
            enemyMovement.SetAnchor(anchorPoint);
        }
        else
        {
            Instantiate(enemyPF, spawnPosition, Quaternion.identity);
        }
    }

    private GameObject GetEnemyPrefab(EnemyType type) {
        return type switch {
            EnemyType.PrototypeEnemy => prototypeEnemyPrefab,
            EnemyType.Fighter1 => fighter1Prefab,
            EnemyType.Fighter2 => fighter2Prefab,
            EnemyType.Fighter3 => fighter3Prefab,
            EnemyType.Bomber => bomberPrefab,
            EnemyType.HomingMissile => homingMissilePrefab,
            EnemyType.EliteEnemy => eliteEnemyPrefab,
            EnemyType.LaserEnemy => laserEnemyPrefab,
            EnemyType.ShotgunEnemy => shotgunEnemyPrefab,
            _ => null
        };
    }
}
