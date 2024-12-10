using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScriptableObjectArchitecture;
using UnityEngine;
public class PlayerHealth : Health {
    [SerializeField] public float damageInvulnerableTime;

    [Header("Components")] 
    [SerializeField] AudioClip hitSound;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private GameEvent playerTookDamage;
    
    // state 
    private float _damageInvulnerableTimer;
    public bool Invulnerable => _damageInvulnerableTimer > 0;

    protected override void Start() {
        base.Start();
        playerInfo.currentHealth = playerInfo.maxHealth;
        playerInfo.playerInvulnerableTime = damageInvulnerableTime;
    }

    private void Update() {
        if (Invulnerable) {
            _damageInvulnerableTimer -= Time.deltaTime;
        }
    }

    public override void TakeDamage(float damage) {
        if (Invulnerable) return;
        
        base.TakeDamage(damage);
        playerInfo.currentHealthPercentage = HealthPercentage;
        playerInfo.currentHealth--;

        SetInvulnerable();
        
        CameraShake.Instance.Shake();
        AudioManager.Instance.Play(hitSound, 1.0f);
        
        playerTookDamage.Raise();
    }

    private void SetInvulnerable() {
        _damageInvulnerableTimer = damageInvulnerableTime;
    }

    protected override void DeathEffect() {
        GameManager.Instance.Reload();
    }
}