using System;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PassiveHeatChange : MonoBehaviour {
    [SerializeField] private FloatVariable heat;
    [SerializeField] private HeatConstants heatConstants;

    private void Start() {
        heat.Value = 0;
    }

    private void Update() {
        heat.Value += Time.deltaTime * heatConstants.passiveHeatDelta;
        if (heat.Value < 0) {
            heat.Value = 0;
        }
        else if (heat.Value > 1) {
            heat.Value = 1;
        } 
    }
}