using System;
using ScriptableObjectArchitecture;
using TMPro;
using UnityEngine;

public class PurchaseGun : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private IntVariable currentMoney;
    [SerializeField] private GunConstants gunConstants;
    [SerializeField] private string gunNameText;

    private Color _originalColor;
    
    private void Start() {
        _originalColor = text.color;
        text.text = gunNameText + " (" + gunConstants.cost + " SOL)";
    }

    public void TryPurchaseGun() {
        if (currentMoney.Value < gunConstants.cost) return;

        currentMoney.Value -= gunConstants.cost;
        gunConstants.MarkOwned();
    }

    private void Update() {
        if (currentMoney.Value < gunConstants.cost) {
            text.color = Color.grey;
        }
        else text.color = _originalColor;
    }
}