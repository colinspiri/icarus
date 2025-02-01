using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GunConstants", menuName = "Scriptable Objects/Gun Constants")]
public class GunConstants : ScriptableObject {
    [Space]
    [Header("References")]
    public HeatConstants heatConstants;
    public EquippedGun equippedGun;

    [Header("Bullet Prefabs")] 
    public GameObject bulletPrefabTier1;
    public GameObject bulletPrefabTier2;
    public GameObject bulletPrefabTier3;
    public GameObject CurrentBulletPrefab => heatConstants.CurrentHeatValue switch {
        HeatValue.Low => bulletPrefabTier1,
        HeatValue.Medium => bulletPrefabTier2,
        HeatValue.High => bulletPrefabTier3,
        _ => throw new ArgumentOutOfRangeException()
    };

    [Header("Damage")]
    public float damageTier1;
    public float damageTier2;
    public float damageTier3;
    public float CurrentDamage => heatConstants.CurrentHeatValue switch {
        HeatValue.Low => damageTier1,
        HeatValue.Medium => damageTier2,
        HeatValue.High => damageTier3,
        _ => throw new ArgumentOutOfRangeException()
    };
    public float DamageFromTier(HeatValue tier) {
        return tier switch {
            HeatValue.Low => damageTier1,
            HeatValue.Medium => damageTier2,
            HeatValue.High => damageTier3,
            _ => throw new ArgumentOutOfRangeException(nameof(tier), tier, null)
        };
    }
    
    [Header("Fire Rate")]
    [Tooltip("Firing rate is 1 / the fire cooldown (in seconds)")]
    public float firingRateTier1;
    public float firingRateTier2;
    public float firingRateTier3;
    public float CurrentFiringRate => heatConstants.CurrentHeatValue switch {
        HeatValue.Low => firingRateTier1,
        HeatValue.Medium => firingRateTier2,
        HeatValue.High => firingRateTier3,
        _ => throw new ArgumentOutOfRangeException()
    };

    [Header("Ammo")]
    public int ammoCostTier1;
    public int ammoCostTier2;
    public int ammoCostTier3;
    public int CurrentAmmoCost => heatConstants.CurrentHeatValue switch {
        HeatValue.Low => ammoCostTier1,
        HeatValue.Medium => ammoCostTier2,
        HeatValue.High => ammoCostTier3,
        _ => throw new ArgumentOutOfRangeException()
    };
    
    [Header("Spread")]
    [Tooltip("Diameter of the spread range, an angle in degrees")]
    public float spreadTier1;
    public float spreadTier2;
    public float spreadTier3;
    public float CurrentSpread => heatConstants.CurrentHeatValue switch {
        HeatValue.Low => spreadTier1,
        HeatValue.Medium => spreadTier2,
        HeatValue.High => spreadTier3,
        _ => throw new ArgumentOutOfRangeException()
    };

    [Header("Progression")] 
    public bool ownedOnStart;
    public int cost;
    
    public bool Owned { get; private set; }
    private string OwnedSaveKey => name + "_Owned";

    public void MarkOwned() {
        Owned = true;
        SaveOwnedValue();
    }
    private void SaveOwnedValue() {
        int intUnlocked = Owned ? 1 : 0;
        PlayerPrefs.SetInt(OwnedSaveKey, intUnlocked);
    }
    
    public bool Equipped { get; private set; }
    private string EquippedSaveKey => name + "_Equipped";

    public void Equip() {
        Equipped = true;
        equippedGun.SetCurrentGun(this);
        SaveEquippedValue();
    }
    public void Unequip() {
        Equipped = false;
        SaveEquippedValue();
    }
    private void SaveEquippedValue() {
        int intEquipped = Equipped ? 1 : 0;
        PlayerPrefs.SetInt(EquippedSaveKey, intEquipped);
    }
    public void LoadSaveState() {
        Equipped = PlayerPrefs.GetInt(EquippedSaveKey, 0) switch {
            0 => false,
            _ => true
        };
        
        int intStartsOwned = ownedOnStart ? 1 : 0;
        Owned = PlayerPrefs.GetInt(OwnedSaveKey, intStartsOwned) switch {
            0 => false,
            _ => true
        };
    }
}