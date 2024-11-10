using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayEffectsOnHeatChange : MonoBehaviour {
    [SerializeField] private HeatConstants heatConstants;
    
    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem lowHeatEffect;
    [SerializeField] private ParticleSystem mediumHeatEffect;
    [SerializeField] private ParticleSystem highHeatEffect;

    private HeatValue _currentHeatValue = HeatValue.Low;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeatValue(heatConstants.CurrentHeatValue);
    }

    private void UpdateHeatValue(HeatValue newHeatValue) {
        if (_currentHeatValue == newHeatValue) return;

        _currentHeatValue = newHeatValue;
        if (_currentHeatValue == HeatValue.High) {
            if(highHeatEffect != null) highHeatEffect.Play();
        }
        else if (_currentHeatValue == HeatValue.Medium) {
            if(mediumHeatEffect != null) mediumHeatEffect.Play();
        }
        else if (_currentHeatValue == HeatValue.Low) {
            if(lowHeatEffect != null) lowHeatEffect.Play();
        }
    }
}
