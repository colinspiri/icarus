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
}