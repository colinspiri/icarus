using UnityEngine;

public class EnemyHealth : Health {
    [SerializeField] private GameObject explosionEffectPrefab;
    
    protected override void DeathEffect() {
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}