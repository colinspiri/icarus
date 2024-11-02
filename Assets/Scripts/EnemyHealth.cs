using UnityEngine;

public class EnemyHealth : Health {
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private AudioSource deathSFX;
    
    protected override void DeathEffect() {
        deathSFX.Play();
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}