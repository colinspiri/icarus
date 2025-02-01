using System;
using UnityEngine;
using Yarn.Unity;

[CreateAssetMenu(fileName = "EquippedGun", menuName = "Scriptable Objects/EquippedGun", order = 0)]
public class EquippedGun : ScriptableObject {
    [SerializeField] private GunConstants currentGun;
    public GunConstants CurrentGun => currentGun;

    public GunConstants defaultGun;

    public void SetCurrentGun(GunConstants newGun) {
        if (currentGun != null) {
            currentGun.Unequip();
        }
        
        currentGun = newGun;
    }

    public void EquipDefaultGun() {
        defaultGun.Equip();
    }
}