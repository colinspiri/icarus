using UnityEngine;
using UnityEngine.Serialization;

public class DamagePlayerOnContact : MonoBehaviour
{
    // components
    private Health _healthComponent;
    
    // constants
    [Tooltip("Damage dealt to player on contact")]
    [FormerlySerializedAs("playerDamage")]
    [SerializeField] private float damageToPlayer = 1f;
    [Tooltip("Damage dealt to this object on contact with player")] 
    [SerializeField] private float damageToSelf = 1f;

    private void Awake() {
        _healthComponent = GetComponent<Health>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player")) {
            var playerHealth = col.gameObject.GetComponent<PlayerHealth>();
            if (PlayerMovement.Instance.isDashing || playerHealth.Invulnerable) return;
            
            // cause damage to player and self
            playerHealth.TakeDamage(damageToPlayer);
            if (_healthComponent) {
                _healthComponent.TakeDamage(damageToSelf);
            }
        }
    }
}
