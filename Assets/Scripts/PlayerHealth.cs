using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
public class PlayerHealth : Health {
    [SerializeField] AudioClip hitSound;
    [SerializeField] private GameEvent playerTookDamage;
    [SerializeField] private PlayerInfo playerInfo;
    
    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        
        CameraShake.Instance.Shake();
        AudioManager.Instance.Play(hitSound, 1.0f);
        playerInfo.currentHealthPercentage = HealthPercentage;
        playerTookDamage.Raise();
    }

    protected override void DeathEffect() {
        GameManager.Instance.Reload();
    }
}