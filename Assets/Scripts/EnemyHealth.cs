using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class EnemyHealth : Health {
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private AudioClip enemyDie;
    [SerializeField] private float heatGain;
    [SerializeField] private FloatVariable heat;
    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);

        // check for hitstop
        if (HealthPercentage <= 0) {
            Hitstop.Instance.NotifyEnemyDeath();
        }
        else Hitstop.Instance.NotifyEnemyTakeDamage();
    }

    protected override void DeathEffect() {
        AudioManager.Instance.Play(enemyDie, .6f);
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        heat.Value += heatGain;
        Destroy(gameObject);
    }
}