using System;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PassiveHeatChange : MonoBehaviour {
    [SerializeField] private FloatVariable heat;
    [SerializeField] private HeatConstants heatConstants;

    private void Start() {
        heatConstants.SetCurrentHeat(0);
        //heat.Value = 0;
    }

    private void Update() {
        heatConstants.CalculateCurrentHeat(Time.deltaTime * heatConstants.CurrentPassiveHeatDelta);
        //heat.Value += Time.deltaTime * heatConstants.CurrentPassiveHeatDelta;
        if (heat.Value < 0) {
            heatConstants.SetCurrentHeat(0);
            //heat.Value = 0;
        }
        else if (heat.Value > 1) {
            heatConstants.SetCurrentHeat(1);
            //heat.Value = 1;
        } 
    }
}