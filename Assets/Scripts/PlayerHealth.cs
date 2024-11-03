using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : Health {
    [SerializeField] AudioClip hitSound;
    
    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        CameraShake.Instance.Shake();
        AudioManager.Instance.Play(hitSound, 1.0f);
    }

    protected override void DeathEffect() {
        GameManager.Instance.Reload();
    }
}