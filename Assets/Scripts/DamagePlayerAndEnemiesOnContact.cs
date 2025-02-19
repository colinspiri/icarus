using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerAndEnemiesOnContact : DamagePlayerOnContact
{
    [Tooltip("Damage dealt to enemies on contact")]
    [SerializeField] private float damageToEnemies = 1f;
    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);

        if (col.CompareTag("Enemy"))
        {
            var enemyHealth = col.gameObject.GetComponent<Health>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(damageToEnemies);
            }
        }
    }
}
