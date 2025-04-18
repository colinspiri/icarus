﻿using UnityEngine;

public class EnemyColorController : ColorController {
    // components
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private EquippedGun equippedGun;
    [Space]
    
    // params 
    [SerializeField] private Color damagedColor;
    [SerializeField] private float damagedTime;
    [Space] 
    [SerializeField] private Color lowHealthColor;
    [SerializeField] private float lowHealthFlashPeriod;
    
    // state
    private bool _takenDamage;
    
    protected override void Start() {
        base.Start();
        enemyHealth.OnTakeDamageAction += () => {
            base.SetTemporaryColor(damagedColor, damagedTime);

            _takenDamage = true;
        };
    }

    protected override void Update() {
        if (_takenDamage) {
            var hitsToKillWithCurrentGun = Mathf.Ceil(enemyHealth.CurrentHealth / equippedGun.CurrentGun.CurrentDamage);
            if (hitsToKillWithCurrentGun <= 1) {
                base.SetFlashing(lowHealthColor, lowHealthFlashPeriod, -1);
            }
            else {
                base.StopFlashing();
            }
        }

        base.Update();
    }
}