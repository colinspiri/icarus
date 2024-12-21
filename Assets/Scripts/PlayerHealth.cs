using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ScriptableObjectArchitecture;
using UnityEngine;
public class PlayerHealth : Health {
    [SerializeField] private float damageInvulnerableTime;
    [SerializeField] private float afterDashInvulnerableTime;

    [Header("Components")] 
    [SerializeField] AudioClip hitSound;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private GameEvent playerTookDamage;
    [SerializeField] private HeatConstants heatConstants;
    [SerializeField] private FloatVariable heat;
    
    // state 
    private float _damageInvulnerableTimer;
    public bool Invulnerable => _damageInvulnerableTimer > 0;

    public event Action<float> OnStartInvulnerable;

    protected override void Start() {
        base.Start();
        
        playerInfo.currentHealth = playerInfo.maxHealth;
        playerInfo.playerInvulnerableTime = damageInvulnerableTime;

        PlayerMovement.Instance.OnDashEnded += () => {
            SetInvulnerable(afterDashInvulnerableTime);
        };
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

        SetInvulnerable(damageInvulnerableTime);
        
        CameraShake.Instance.Shake();
        AudioManager.Instance.Play(hitSound, 1.0f);

        heat.Value -= heatConstants.heatCostOnPlayerDamaged;
        if (heat.Value < 0) heat.Value = 0;
        
        playerTookDamage.Raise();
    }

    private void SetInvulnerable(float time) {
        _damageInvulnerableTimer = time;
        OnStartInvulnerable?.Invoke(time);
    }

    protected override void DeathEffect() {
        GameManager.Instance.ReloadScene();
    }
}