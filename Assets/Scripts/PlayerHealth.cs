using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHealth : Health {
    
    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);
        CameraShake.Instance.Shake();
        GetComponent<AudioSource>().Play();
    }

    protected override void DeathEffect() {
        GameManager.Instance.Reload();
    }
}