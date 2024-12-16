using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletOriginObject;

    [Header("Shooting Mode")]
    [SerializeField] private ShootingMode shootingMode;
    private enum ShootingMode
    {
        SingleBullet,
        Pattern,
        // HomingMissile
    };
    [SerializeField] private float firingRate;
    [SerializeField] private float randomCooldownVariance = 0.3f;
    [Tooltip("Speed at which it rotates to face the player")]
    [SerializeField] private float rotationSpeed;

    [Header("Single Bullet")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("Pattern")]
    [SerializeField] private GameObject patternPrefab;
    [Tooltip("If relative to enemy rotation, right direction of pattern will be aligned to right direction (forward) of enemy.")]
    [SerializeField] private bool relativeToEnemyRotation;

    /*[Header("Homing Missile")]
    [SerializeField] private GameObject homingMissilePrefab;
    [SerializeField] private bool hasHomingMissileAbility = false;
    [SerializeField] private float homingMissileMinDistance = 8f;*/

    [Header("Audio")]
    [SerializeField] private AudioClip enemyShoot;

    // state
    private float _currentFiringCooldown;
    private bool _canFire;

    // Start is called before the first frame update
    void Start()
    {
        StartFiringCooldown();
        _canFire = true;
    }

    // Update is called once per frame
    void Update()
    {
        LookAtPoint(PlayerMovement.Instance.transform.position);

        if (_currentFiringCooldown <= 0 && _canFire)
        {
            Fire();
            StartFiringCooldown();
        }
        else _currentFiringCooldown -= Time.deltaTime;

        /*if (hasHomingMissileAbility && shootingMode == ShootingMode.SingleBullet && 
            Vector2.Distance(transform.position, PlayerMovement.Instance.transform.position) >= homingMissileMinDistance){
            shootingMode = ShootingMode.HomingMissile;
        }

        if (hasHomingMissileAbility && shootingMode == ShootingMode.HomingMissile && 
            Vector2.Distance(transform.position, PlayerMovement.Instance.transform.position) < homingMissileMinDistance){
            shootingMode = ShootingMode.SingleBullet;
        }*/
    }

    private void StartFiringCooldown() {
        float firingCooldown = 1.0f / firingRate;
        float variance = Random.Range(-randomCooldownVariance*0.5f, 0.5f*randomCooldownVariance);
        _currentFiringCooldown = firingCooldown + variance;
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

    private void Fire()
    {
        if (shootingMode == ShootingMode.SingleBullet)
        {
            FireBullet();
        }
        else if (shootingMode == ShootingMode.Pattern)
        {
            FirePattern();
        }
        /*else if (shootingMode == ShootingMode.HomingMissile)
        {
            FireHomingMissile();
        }*/
    }

    private void FireBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.originObject = bulletOriginObject;

        AudioManager.Instance.Play(enemyShoot, 0.1f);
    }

    private void FirePattern()
    {
        Instantiate(patternPrefab, bulletSpawnPoint.position,
            relativeToEnemyRotation ? bulletSpawnPoint.rotation : Quaternion.identity);

        AudioManager.Instance.Play(enemyShoot, 0.1f);
    }

    /*private void FireHomingMissile()
    {
        Instantiate(homingMissilePrefab, bulletSpawnPoint.position,
            relativeToEnemyRotation ? bulletSpawnPoint.rotation : Quaternion.identity);

        AudioManager.Instance.Play(enemyShoot, 0.1f);
    }*/

    public void SetCanFire(bool canFire) => _canFire = canFire;
}
