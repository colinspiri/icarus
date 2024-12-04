using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(fileName = "HeatConstants", menuName = "Scriptable Objects/Heat Constants")]
public class HeatConstants : ScriptableObject {
    public FloatReference currentHeat;
    
    [Header("Heat Thresholds")]
    public float mediumHeatThreshold; 
    public float highHeatThreshold; 
    public HeatValue CurrentHeatValue => 
        (currentHeat.Value >= highHeatThreshold)
            ? HeatValue.High : 
            (currentHeat.Value >= mediumHeatThreshold) 
                ? HeatValue.Medium : HeatValue.Low;

    [Tooltip("How fast heat increases/decreases over time")]
    [Space]
    public float passiveHeatDeltaLow;
    public float passiveHeatDeltaMedium;
    public float passiveHeatDeltaHigh;
    public float CurrentPassiveHeatDelta => CurrentHeatValue switch {
        HeatValue.Low => passiveHeatDeltaLow,
        HeatValue.Medium => passiveHeatDeltaMedium,
        HeatValue.High => passiveHeatDeltaHigh,
        _ => passiveHeatDeltaLow,
    };

    [Header("Heat Cost Per Shot")] 
    public float heatCostPerShotLow; 
    public float heatCostPerShotMedium; 
    public float heatCostPerShotHigh; 
    public float CurrentHeatCostPerShot => CurrentHeatValue switch {
        HeatValue.Low => heatCostPerShotLow,
        HeatValue.Medium => heatCostPerShotMedium,
        HeatValue.High => heatCostPerShotHigh,
        _ => heatCostPerShotLow,
    };

    [Space] 
    public float heatCostPerDash;
    public float heatCostPerReloadClipEmpty; // 0.15
    public float heatCostPerReloadClipFull; // 0.05

    [Header("Heat on Enemy Kill")] 
    public float heatGainOnEnemyKill;

    [Header("Heat Gain")] 
    public float heatGainPerDamage;
}
public enum HeatValue { Low, Medium, High }
