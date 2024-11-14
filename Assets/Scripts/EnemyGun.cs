using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletOriginObject;

    [Header("Shooting Mode")]
    [SerializeField] private ShootingMode shootingMode;
    private enum ShootingMode {
        SingleBullet,
        Pattern
    };
    [SerializeField] private float staticFiringCooldown;
    [SerializeField] private bool useRandomFiringCooldown;
    [SerializeField] private MinMaxFloat randomFiringCooldown;
    [Tooltip("Speed at which it rotates to face the player")]
    [SerializeField] private float rotationSpeed;

    [Header("Single Bullet")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Pattern")] 
    [SerializeField] private GameObject patternPrefab;
    [Tooltip("If relative to enemy rotation, right direction of pattern will be aligned to right direction (forward) of enemy.")]
    [SerializeField] private bool relativeToEnemyRotation;
    
    [Header("Audio")]
    [SerializeField] private AudioClip enemyShoot;

    // state
    private float _currentFiringCooldown;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPoint(PlayerMovement.Instance.transform.position);

        if (_currentFiringCooldown <= 0) {
            Fire();
            _currentFiringCooldown = useRandomFiringCooldown ? randomFiringCooldown.RandomValue : staticFiringCooldown;
        }
        else _currentFiringCooldown -= Time.deltaTime;
    }

    private void LookAtPoint(Vector3 point)
    {
        if (Time.timeScale > 0)
        {
            // rotate to look at the point
            float angle = Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Fire() {
        if (shootingMode == ShootingMode.SingleBullet) {
            FireBullet();
        }
        else if (shootingMode == ShootingMode.Pattern) {
            FirePattern();
        }
    }
    
    private void FireBullet() {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.originObject = bulletOriginObject;
        
        AudioManager.Instance.Play(enemyShoot, 0.1f);
    }

    private void FirePattern() {
        Instantiate(patternPrefab, bulletSpawnPoint.position,
            relativeToEnemyRotation ? bulletSpawnPoint.rotation : Quaternion.identity);

        AudioManager.Instance.Play(enemyShoot, 0.1f);
    }
}
