using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class EnemyHealth : Health {
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private AudioClip enemyDie;
    [SerializeField] private float heatGain;
    [SerializeField] private FloatVariable heat;
    [Space] 
    [SerializeField] private bool hitstopOnDamage;
    [SerializeField] private float hitstopTimeOnDamage;
    [SerializeField] private bool hitstopOnDeath;
    [SerializeField] private float hitstopTimeOnDeath;

    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);

        if ((HealthPercentage <= 0 && hitstopOnDeath)) {
            Hitstop.Instance.DoHitstop(hitstopTimeOnDeath);
        }
        else if (hitstopOnDamage) {
            Hitstop.Instance.DoHitstop(hitstopTimeOnDamage);
        }
    }

    protected override void DeathEffect() {
        AudioManager.Instance.Play(enemyDie, .6f);
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        heat.Value += heatGain;
        Destroy(gameObject);
    }
}