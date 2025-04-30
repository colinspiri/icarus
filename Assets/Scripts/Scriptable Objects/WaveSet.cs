using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSet", menuName = "Scriptable Objects/WaveSet")]
public class WaveSet : ScriptableObject {
    public List<Wave> waves;
    public int NumWaves => waves.Count;
}

[Serializable]
public struct Wave {
    [Header("Wave Parameters")] 
    public List<NumEnemies> enemies;
    public bool hasStaticObjectObstacles;
    public float staticObjectSpawnFrequency;
    public float spawnTimerVariance;
    [Tooltip("Add more of the same prefab to increase its chance of spawning")]
    public List<GameObject> staticObjectPrefabs;
}

[Serializable]
public struct NumEnemies {
    public EnemyType enemyType;
    public int count;
}

public enum EnemyType
{
    PrototypeEnemy,
    Fighter1,
    Fighter2,
    Fighter3,
    Bomber,
    HomingMissile,
    EliteEnemy,
    LaserEnemy,
    ShotgunEnemy,
    MachineGunEnemy,
    ArcEnemy,
    OrbitalLaser,
    OrbitalLaserAlwaysOn,
    OrbitalLaserFast,
    OrbitalLaserSlow,
    LaserGunEnemy,
}