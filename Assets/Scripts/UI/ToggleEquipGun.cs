using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleEquipGun : MonoBehaviour {
    [SerializeField] private GunConstants gunConstants;
    
    private Toggle _toggle;

    private void Awake() {
        _toggle = GetComponent<Toggle>();
    }

    private void OnEnable() {
        gunConstants.LoadEquippedValue();
        UpdateToggleValue();
    }
    
    private void Update() {
        UpdateToggleValue();
    }

    private void UpdateToggleValue() {
        bool toggleEnabled = gunConstants.Equipped;
        if (_toggle.isOn != toggleEnabled) {
            _toggle.SetIsOnWithoutNotify(toggleEnabled);
        }
    }
    
    public void SetValue(bool newValue) {
        if (newValue) {
            gunConstants.Equip();
        }
    }
}
