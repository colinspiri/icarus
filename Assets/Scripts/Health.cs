using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour {
    [SerializeField] public float maxHealth = 1;

    public float HealthPercentage => CurrentHealth / maxHealth;
    public float CurrentHealth { get; private set; }

    [SerializeField] protected UnityEvent onTakeDamage;
    public event Action OnTakeDamageAction;

    // Start is called before the first frame update
    protected virtual void Start() {
        CurrentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage) {
        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;
        
        onTakeDamage?.Invoke();
        OnTakeDamageAction?.Invoke();

        if (CurrentHealth <= 0) {
            DeathEffect();
        }
    }

    protected abstract void DeathEffect();
}
