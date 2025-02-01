using System;
using UnityEngine;
using Yarn.Unity;

[CreateAssetMenu(fileName = "EquippedGun", menuName = "Scriptable Objects/EquippedGun", order = 0)]
public class EquippedGun : ScriptableObject {
    [SerializeField] private GunConstants currentGun;
    public GunConstants CurrentGun => currentGun;

    public GunConstants defaultGun;
    public SerializedDictionary<string, GunConstants> gunsByPlayerPrefKey;

    public void SetCurrentGun(GunConstants newGun) {
        currentGun = newGun;
    }

    public void SaveValue() {
        foreach (var pair in gunsByPlayerPrefKey) {
            if (pair.Value == currentGun) {
                PlayerPrefs.SetString("EquippedGun", pair.Key);
                return;
            }
        }
    }
    public void LoadValueFromPlayerPrefs() {
        var savedGun = PlayerPrefs.GetString("EquippedGun", "");
        if (savedGun != String.Empty) {
            currentGun = gunsByPlayerPrefKey[savedGun];
        }
        else {
            currentGun = defaultGun;
        }
    }
}