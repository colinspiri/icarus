using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Tooltip("Damage dealt to player when enemy comes in contact")]
    [SerializeField] private float playerDamage = 1f;

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")) DamagePlayer(col);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")) DamagePlayer(col);
    }

    private void DamagePlayer(Collider2D col)
    {
        var playerHealth = col.gameObject.GetComponent<PlayerHealth>();
        if (!playerHealth.Invulnerable) playerHealth.TakeDamage(playerDamage);
    }
}
