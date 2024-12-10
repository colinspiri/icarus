using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour {
    [SerializeField] private float maxHealth = 1;

    private float _currentHealth;

    public float HealthPercentage => _currentHealth / maxHealth;
    public int HealthLeft => (int)Mathf.Ceil(_currentHealth);
    
    [SerializeField] protected UnityEvent onTakeDamage;
    public event Action OnTakeDamageAction;

    // Start is called before the first frame update
    protected virtual void Start() {
        _currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage) {
        _currentHealth -= damage;
        if (_currentHealth < 0) _currentHealth = 0;
        
        onTakeDamage?.Invoke();
        OnTakeDamageAction?.Invoke();

        if (_currentHealth <= 0) {
            DeathEffect();
        }
    }

    protected abstract void DeathEffect();
}
