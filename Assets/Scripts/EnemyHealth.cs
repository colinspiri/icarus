using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyHealth : Health {
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private AudioClip enemyDie;
    [SerializeField] private float heatGain;
    [SerializeField] private FloatVariable heat;
    


    protected override void DeathEffect() {
        AudioManager.Instance.Play(enemyDie, .6f);
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        heat.Value += heatGain;
        Destroy(gameObject);
       
    }
}