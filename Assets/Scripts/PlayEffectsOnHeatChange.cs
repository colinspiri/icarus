using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayEffectsOnHeatChange : MonoBehaviour {
    [SerializeField] private HeatConstants heatConstants;
    
    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem lowToMediumHeat;
    [SerializeField] private ParticleSystem mediumToHighHeat;
    [SerializeField] private ParticleSystem highToMediumHeat;
    [SerializeField] private ParticleSystem mediumToLowHeat;

    [SerializeField] private SoundProfile powerUp;
    [SerializeField] private SoundProfile powerDown;

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

        StopAllEffects();
        if (_currentHeatValue == HeatValue.Low && newHeatValue == HeatValue.Medium) {
            lowToMediumHeat.Play();
            powerUp.PlaySFX();
            heatConstants.CalculateCurrentHeat(heatConstants.heatToAddLowToMedium);
        }
        else if (_currentHeatValue == HeatValue.Medium && newHeatValue == HeatValue.High) {
            mediumToHighHeat.Play();
            powerUp.PlaySFX();
            heatConstants.CalculateCurrentHeat(heatConstants.heatToAddMediumToHigh);
        }
        else if (_currentHeatValue == HeatValue.High && newHeatValue == HeatValue.Medium) {
            highToMediumHeat.Play();
            powerDown.PlaySFX();
            heatConstants.CalculateCurrentHeat(heatConstants.heatToAddHighToMedium);
        }
        else if (_currentHeatValue == HeatValue.Medium && newHeatValue == HeatValue.Low) {
            mediumToLowHeat.Play();
            powerDown.PlaySFX();
            heatConstants.CalculateCurrentHeat(heatConstants.heatToAddMediumToLow);
        }

        _currentHeatValue = newHeatValue;
    }

    private void StopAllEffects() {
        lowToMediumHeat.Stop();
        mediumToHighHeat.Stop();
        highToMediumHeat.Stop();
        mediumToLowHeat.Stop();
    }
}
