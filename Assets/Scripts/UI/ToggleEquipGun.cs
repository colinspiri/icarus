using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleEquipGun : MonoBehaviour {
    [SerializeField] private EquippedGun equippedGun;
    [SerializeField] private GunConstants gunConstants;
    
    private Toggle _toggle;

    private void Awake() {
        _toggle = GetComponent<Toggle>();
    }

    private void OnEnable() {
        equippedGun.LoadValueFromPlayerPrefs();
        UpdateToggleValue();
    }
    
    private void Update() {
        UpdateToggleValue();
    }

    private void UpdateToggleValue() {
        bool toggleEnabled = equippedGun.CurrentGun == gunConstants;
        if (_toggle.isOn != toggleEnabled) {
            _toggle.SetIsOnWithoutNotify(toggleEnabled);
        }
    }
    
    public void SetValue(bool newValue) {
        if (newValue) {
            equippedGun.SetCurrentGun(gunConstants);
        }
        
        equippedGun.SaveValue();
    }
}
