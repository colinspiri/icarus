using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float LifeTime { get; set; } = 60f;
    public float MoveSpeed { get; set; } = 1f;
    public float Damage { get; set; } = 1f;

    public GameObject originObject;

    [SerializeField] private GameObject explosionEffectPrefab;

    private Vector2 _spawnPoint;
    private float _lifeTimer;

    private void Start() {
        _spawnPoint = transform.position;
        _lifeTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_lifeTimer > LifeTime) Destroy(gameObject);
        _lifeTimer += Time.deltaTime;
        Move();
    }

    private void Move() {
        transform.Translate(transform.right * (Time.deltaTime * MoveSpeed), Space.World);
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == null) return;
        if (col.gameObject == originObject) return;

        if (col.CompareTag("Bullet")) {
            col.GetComponent<Bullet>().Die();
            Die();
        }
        else if (col.CompareTag("Player") || col.CompareTag("Enemy")) {
            var health = col.GetComponent<Health>();
            health.TakeDamage(Damage);
            Die();
        }
    }

    private void Die() {
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
