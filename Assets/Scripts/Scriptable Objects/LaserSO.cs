using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserSO", menuName = "Scriptable Objects/LaserSO")]
public class LaserSO : ScriptableObject
{
    public float laserDamage;
    public float laserDuration;
    public float laserDamageDuration;
}
