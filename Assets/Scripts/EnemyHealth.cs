using UnityEngine;

public class EnemyHealth : Health {
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private AudioClip enemyDie;
    
    protected override void DeathEffect() {
        AudioManager.Instance.Play(enemyDie, .6f);
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}