using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenu : MonoBehaviour {
    [SerializeField] private List<ShopSlot> shopSlots;
    [SerializeField] private EquippedGun equippedGun;
    
    private void Awake() {
        foreach (var slot in shopSlots) {
            slot.gunConstants.LoadSaveState();
        }
        equippedGun.EquipDefaultGun();
    }
    
    private void OnEnable() {
        DisplayShopSlots();
    }
    private void Start() {
        DisplayShopSlots();
    }

    public void DisplayShopSlots() {
        foreach (var slot in shopSlots) {
            if (!slot.gunConstants.Owned) {
                slot.purchaseButton.SetActive(true);
                slot.equipButton.SetActive(false);
            }
            else {
                slot.purchaseButton.SetActive(false);
                slot.equipButton.SetActive(true);
            }
        }
    }
}

[Serializable]
struct ShopSlot {
    public GunConstants gunConstants;
    public GameObject purchaseButton;
    public GameObject equipButton;
}