using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class EnemyHealth : Health {
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private AudioClip enemyDie;
    [SerializeField] private FloatVariable heat;
    [SerializeField] private HeatConstants heatConstants;
    
    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);

        // check for hitstop
        if (HealthPercentage <= 0) {
            Hitstop.Instance.NotifyEnemyDeath();
        }
        else {
            // Hitstop.Instance.NotifyEnemyTakeDamage();
            CameraShake.Instance.NotifyEnemyTakeDamage();
        }
    }

    protected override void DeathEffect() {
        AudioManager.Instance.Play(enemyDie, .6f);
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        heat.Value += heatConstants.heatGainOnEnemyKill;
        Destroy(gameObject);
    }
}