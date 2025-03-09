using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LaserGunConstants", menuName = "Scriptable Objects/Laser Gun Constants")]
public class LaserGunConstants : GunConstants
{
    [Header("Laser")]
    public float laserDamagePerSecond;
}
