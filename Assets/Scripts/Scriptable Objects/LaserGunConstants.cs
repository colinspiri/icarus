using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserGunConstants", menuName = "Scriptable Objects/Laser Gun Constants")]
public class LaserGunConstants : GunConstants
{
    [Header("Laser")]
    public float laserDamagePerSecond;
    [Tooltip("Laser will automatically fire if it reaches max damage while charging")]
    public float maxDamage;
}
