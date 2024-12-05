using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(fileName = "GunConstants", menuName = "Scriptable Objects/Gun Constants")]
public class GunConstants : ScriptableObject {
    public HeatConstants heatConstants;
    
    [Header("Fire Rate")]
    [Tooltip("Firing rate is 1 / the fire cooldown (in seconds)")]
    public float firingRateTier1;
    public float firingRateTier2;
    public float firingRateTier3;
    public float CurrentFiringRate => heatConstants.CurrentHeatValue switch {
        HeatValue.Low => firingRateTier1,
        HeatValue.Medium => firingRateTier2,
        HeatValue.High => firingRateTier3,
        _ => throw new ArgumentOutOfRangeException()
    };

    [Header("Ammo")]
    public int maxAmmo;
    [Space]
    public int ammoCostTier1;
    public int ammoCostTier2;
    public int ammoCostTier3;
    public int CurrentAmmoCost => heatConstants.CurrentHeatValue switch {
        HeatValue.Low => ammoCostTier1,
        HeatValue.Medium => ammoCostTier2,
        HeatValue.High => ammoCostTier3,
        _ => throw new ArgumentOutOfRangeException()
    };
    [Space] 
    public float reloadTime;
}