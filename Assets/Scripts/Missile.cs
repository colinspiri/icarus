using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Enemy
{
    [Tooltip("Damage dealt to missile when it comes into contact with player")]
    [SerializeField] private float missileDamage = 1f;

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        if (col.gameObject.CompareTag("Player")) DamageMissile();
    }

    private void DamageMissile() => GetComponent<EnemyHealth>().TakeDamage(missileDamage);
}
