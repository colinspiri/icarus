using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBoolVariable : MonoBehaviour {
    [SerializeField] private Toggle toggle;
    [Space] 
    [SerializeField] private BoolVariable boolVariable;
    [SerializeField] private BoolReference defaultValue;
    [Space] 
    [SerializeField] private string playerPrefString;

    private void OnEnable() {
        GetVariableFromPlayerPrefs();
        bool startingVal = boolVariable.Value;
        
        toggle.SetIsOnWithoutNotify(startingVal);
    }

    private void GetVariableFromPlayerPrefs() {
        int defaultValueInt = defaultValue.Value ? 1 : 0;
        defaultValue.Value = PlayerPrefs.GetInt(playerPrefString, defaultValueInt) switch {
            0 => false,
            _ => true
        };
    }

    private void SaveValue() {
        int intValue = boolVariable.Value ? 1 : 0;
        PlayerPrefs.SetInt(playerPrefString, intValue);
    }

    public void SetValue(bool newValue) {
        boolVariable.Value = newValue;

        if (playerPrefString != String.Empty) {
            SaveValue();
        }
    }
}
