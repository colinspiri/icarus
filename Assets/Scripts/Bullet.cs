using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    // components
    [Header("Components")]
    public GameObject originObject;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private AudioClip collideSFX;
    [SerializeField] private AudioClip enemyDamage;
    
    // serialized constants
    [Header("Constants")]
    public bool fromPlayer;
    [Space]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float damage = 1f;
    [Tooltip("Number of bullets to pierce through. -1 for infinite.")]
    [SerializeField] private int bulletPierceMax;
    [Tooltip("Number of entities to pierce through. -1 for infinite.")] 
    [SerializeField] private int entityPierceMax;

    // private state
    private const float LIFE_TIME = 60f;
    private float _lifeTimer;
    private int _bulletPierceCount;
    private int _entityPierceCount;

    private void Start() {
        _lifeTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_lifeTimer > LIFE_TIME) Destroy(gameObject);
        
        _lifeTimer += Time.deltaTime;
        Move();
    }

    private void Move() {
        transform.Translate(transform.right * (Time.deltaTime * bulletSpeed), Space.World);
    }

    public void SetBulletSpeed(float value) {
        bulletSpeed = value;
    }

    private void HitBullet() {
        _bulletPierceCount++;
        if (bulletPierceMax != -1 && _bulletPierceCount > bulletPierceMax) {
            Die();
        }
    }

    private void HitEntity() {
        _entityPierceCount++;
        if (entityPierceMax != -1 && _entityPierceCount > entityPierceMax) {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject == null) return;
        if (col.gameObject == originObject) return;

        if (col.CompareTag("Bullet")) {
            var otherBullet = col.GetComponent<Bullet>();
            if (this.fromPlayer != otherBullet.fromPlayer) {
                AudioManager.Instance.Play(collideSFX, 0.5f);
                otherBullet.HitBullet();
                HitBullet();
            }
        }
        else if (col.CompareTag("Player") && !fromPlayer) {
            if (PlayerMovement.Instance.isDashing) return;
            var health = col.GetComponent<Health>();
            health.TakeDamage(damage);
            HitEntity();
        }
        else if (col.CompareTag("Enemy") && fromPlayer) {
            AudioManager.Instance.Play(enemyDamage, 1.0f);
            var health = col.GetComponent<Health>();
            health.TakeDamage(damage);
            HitEntity();
        }
    }

    private void Die() {
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
