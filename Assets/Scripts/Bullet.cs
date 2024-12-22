using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Serialization;

public class Bullet : MonoBehaviour {
    // components
    [Header("Components")]
    public GameObject originObject;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private FloatVariable heat;
    [SerializeField] private HeatConstants heatConstants;

    // serialized constants
    [Header("Constants")]
    public bool fromPlayer;
    
    [Header("Speed")]
    [SerializeField] private float staticBulletSpeed;
    [SerializeField] private bool useRandomBulletSpeed;
    [SerializeField] private MinMaxFloat randomBulletSpeed;
    
    [Header("Damage")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private bool useValueFromGunConstants;
    [SerializeField] private GunConstants gunConstants;
    [SerializeField] private HeatValue tier;
    private float Damage => useValueFromGunConstants ? gunConstants.DamageFromTier(tier) : damage;

    [Header("Piercing")]
    [Tooltip("Number of bullets to pierce through. -1 for infinite.")]
    [SerializeField] private int bulletPierceMax;
    [Tooltip("Number of entities to pierce through. -1 for infinite.")] 
    [SerializeField] private int entityPierceMax;
    
    [Header("Life Time")]
    [SerializeField] private float lifeTime = 30f;
    [SerializeField] private float lifeTimeDistance = -1;
    [SerializeField] private bool reduceSizeOverLifeTime;
    [SerializeField] private AnimationCurve sizeCurve;
    [SerializeField] private bool reduceSpeedOverLifeTime;
    [SerializeField] private AnimationCurve speedCurve;
    
    [Header("When Reflected")]
    [SerializeField] private Color reflectedColor;
    [FormerlySerializedAs("bulletSpeedMultiplier")] [SerializeField] private float reflectedSpeedMultiplier;
    [SerializeField] private float reflectedDamage = 1f;

    // state
    [HideInInspector] public bool reflected;
    
    [Header("Audio")]
    [SerializeField] private AudioClip collideSFX;
    [SerializeField] private AudioClip enemyDamage;

    // private state
    private int _bulletPierceCount;
    private int _entityPierceCount;
    private float _elapsedTime;
    private float _lifeTimePercent;
    private Vector3 _originalScale;
    private List<GameObject> _ignoreCollisionsWithObjects = new List<GameObject>();

    private void Start() {
        _elapsedTime = 0;
        _originalScale = transform.localScale;
        reflected = false;
    }

    // Update is called once per frame
    void Update() {
        UpdateLifeTime();
        
        Move();
    }

    private void UpdateLifeTime() {
        var elapsedDistance = _elapsedTime * GetCurrentSpeed();
        if (_elapsedTime > lifeTime || (lifeTimeDistance != -1 && elapsedDistance > lifeTimeDistance)) {
            Destroy(gameObject);
            return;
        }
        _elapsedTime += Time.deltaTime;
        
        var timeT = _elapsedTime / lifeTime;
        var distanceT = elapsedDistance / lifeTimeDistance;
        _lifeTimePercent = lifeTimeDistance == -1 ? timeT : Mathf.Max(timeT, distanceT);

        if (reduceSizeOverLifeTime) {
            var scaleFactor = sizeCurve.Evaluate(_lifeTimePercent);
            transform.localScale = scaleFactor * _originalScale;
        }
    }

    private void Move() {
        transform.Translate(transform.right * (Time.deltaTime * GetCurrentSpeed()), Space.World);
    }

    private float GetCurrentSpeed() {
        var speed = useRandomBulletSpeed ? randomBulletSpeed.RandomValue : staticBulletSpeed;
        if (reflected) {
            speed *= reflectedSpeedMultiplier;
        }

        if (reduceSpeedOverLifeTime) {
            var factor = speedCurve.Evaluate(_lifeTimePercent);
            speed *= factor;
        }

        return speed;
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
        // ignore collisions
        if (col.gameObject == null) return;
        if (col.gameObject == originObject && !reflected) return;
        if (_ignoreCollisionsWithObjects.Contains(col.gameObject)) return;

        if (col.CompareTag("Bullet")) {
            var otherBullet = col.GetComponent<Bullet>();
            if (this.fromPlayer != otherBullet.fromPlayer) {
                AudioManager.Instance.Play(collideSFX, 0.5f);
                HitBullet();
                _ignoreCollisionsWithObjects.Add(otherBullet.gameObject);
                otherBullet.HitBullet();
                otherBullet._ignoreCollisionsWithObjects.Add(this.gameObject);
            }
        }
        else if (col.CompareTag("Player") && !fromPlayer) {
            if (!PlayerMovement.Instance.isDashing)
            {
                var health = col.GetComponent<Health>();
                health.TakeDamage(Damage);
                HitEntity();
                // ignore future collisions with player even after dash is over
                _ignoreCollisionsWithObjects.Add(col.gameObject); 
            }
            else Destroy(gameObject);
        }
        else if (col.CompareTag("Enemy") && (fromPlayer || reflected)) {
            AudioManager.Instance.Play(enemyDamage, 1.0f);

            var chosenDamage = reflected ? reflectedDamage : Damage;
            var health = col.GetComponent<Health>();
            health.TakeDamage(chosenDamage);
            heat.Value += heatConstants.heatGainPerDamage * chosenDamage;
            
            HitEntity();
            _ignoreCollisionsWithObjects.Add(col.gameObject);
        }
    }

    public void Reflect() {
        reflected = true;
        spriteRenderer.color = reflectedColor;
        transform.right = -transform.right;
    }

    private void Die() {
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
