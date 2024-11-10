using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

[CreateAssetMenu(fileName = "HeatConstants", menuName = "Scriptable Objects/Heat Constants")]
public class HeatConstants : ScriptableObject {
    public FloatReference currentHeat;
    
    [Tooltip("How fast heat increases/decreases over time")]
    public float passiveHeatDelta; // -0.03
    [Space]
    public float mediumHeatThreshold; // 0.33
    public float highHeatThreshold; // 0.7
    [Space] 
    public float heatCostPerShotLow; // 0.40
    public float heatCostPerShotMedium; // 0.15
    public float heatCostPerShotHigh; // 0.03
    [Space] 
    public float heatCostPerDash;
    
    public HeatValue CurrentHeatValue => currentHeat.Value >= highHeatThreshold
        ? HeatValue.High
        : (currentHeat.Value >= mediumHeatThreshold ? HeatValue.Medium : HeatValue.Low);
}
public enum HeatValue { Low, Medium, High }
