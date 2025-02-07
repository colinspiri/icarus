using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class EnemyHealth : Health {
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private SoundProfile enemyDie;
    [SerializeField] private FloatVariable heat;
    [SerializeField] private HeatConstants heatConstants;
    [Space]
    [SerializeField] private ParticleSystem damagedParticles;
    
    public override void TakeDamage(float damage) {
        base.TakeDamage(damage);

        // check for hitstop
        if (HealthPercentage <= 0) {
            Hitstop.Instance.NotifyEnemyDeath(maxHealth);
        }
        else {
            // Hitstop.Instance.NotifyEnemyTakeDamage();
            CameraShake.Instance.NotifyEnemyTakeDamage();
        }
        
        // play particles
        if(HealthPercentage <= 0.5) {
            if(!damagedParticles.isPlaying) damagedParticles.Play();
        }
        else damagedParticles.Stop();
    }

    protected override void DeathEffect() {
        enemyDie.PlaySFX();
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        heatConstants.CalculateCurrentHeat(heatConstants.heatGainOnEnemyKill);
        //heat.Value += heatConstants.heatGainOnEnemyKill;
        Destroy(gameObject);
    }
}