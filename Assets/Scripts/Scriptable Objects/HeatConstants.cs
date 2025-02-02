using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(fileName = "HeatConstants", menuName = "Scriptable Objects/Heat Constants")]
public class HeatConstants : ScriptableObject {
    public FloatReference currentHeat;
    public FloatReference currentHeatRelativeToTier;
    public IntReference tier;

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

    [Header("Heat Cost")] 
    public float heatCostPerShotLow; 
    public float heatCostPerShotMedium; 
    public float heatCostPerShotHigh; 
    public float CurrentHeatCostPerShot => CurrentHeatValue switch {
        HeatValue.Low => heatCostPerShotLow,
        HeatValue.Medium => heatCostPerShotMedium,
        HeatValue.High => heatCostPerShotHigh,
        _ => heatCostPerShotLow,
    };

    [Header("Heat to Add When Passing Thresholds")]
    public float heatToAddLowToMedium;
    public float heatToAddMediumToHigh;
    public float heatToAddHighToMedium;
    public float heatToAddMediumToLow;

    [Space] 
    public float heatCostPerDash;
    [Space] 
    public float heatCostOnPlayerDamaged;
    [Space]
    public float heatCostPerReloadClipEmpty; // 0.15
    public float heatCostPerReloadClipFull; // 0.05

    [Header("Heat Gain")] 
    public float heatGainOnEnemyKill;

    [Space]
    public float heatGainPerDamage;

    /// <summary>
    /// Calculates the current heat value by adding the specified heat difference to the current heat value.
    /// </summary>
    /// <param name="heatDifference">The heat difference to add to the current heat value.</param>
    public void CalculateCurrentHeat(float heatDifference)
    {
        currentHeat.Value += heatDifference;
        CalculateHeatRelativeToTier();
    }

    public void SetCurrentHeat(float heat)
    {
        currentHeat.Value = heat;
        CalculateHeatRelativeToTier();
    }

    private void CalculateHeatRelativeToTier()
    {
        currentHeatRelativeToTier.Value = CurrentHeatValue switch
        {
            HeatValue.Low => currentHeat.Value / mediumHeatThreshold,
            HeatValue.Medium => (currentHeat.Value - mediumHeatThreshold) / (highHeatThreshold - mediumHeatThreshold),
            HeatValue.High => (currentHeat.Value - highHeatThreshold) / (1 - highHeatThreshold),
            _ => 0,
        };

        tier.Value = CurrentHeatValue switch
        {
            HeatValue.Low => 1,
            HeatValue.Medium => 2,
            HeatValue.High => 3,
            _ => 1,
        };
    }
}
public enum HeatValue { Low, Medium, High }
