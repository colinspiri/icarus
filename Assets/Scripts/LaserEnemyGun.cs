using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LaserEnemyGun : EnemyGun
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserDuration = 3.0f;
    [SerializeField] private float laserDamageDuration = 0.5f;
    [SerializeField] private float laserDamage = 0.3f;

    private Laser laser = null;

    protected override void Update()
    {
        if (_canFire) LookAtPoint(PlayerMovement.Instance.transform.position);

        if (_currentFiringCooldown <= 0 && _canFire && IsFacingPlayer()) FireLaser();
        else _currentFiringCooldown -= Time.deltaTime;
    }

    private void FireLaser()
    {
        _canFire = false;
        StartFiringCooldown();
        var enemyMovement = bulletOriginObject.GetComponent<EnemyMovement>();
        enemyMovement.PauseMovement(laserDuration + laserDamageDuration);

        var spriteRenderer = laserPrefab.GetComponent<SpriteRenderer>();
        var adjustedBulletSpawnPoint = bulletSpawnPoint.position + bulletSpawnPoint.right * spriteRenderer.bounds.size.x / 2; 
        laser = Instantiate(laserPrefab, adjustedBulletSpawnPoint, bulletSpawnPoint.rotation).GetComponent<Laser>();
    }

    public void SetCanFire(bool canFire)
    {
        _canFire = canFire;
    }

    private void OnDestroy()
    {
       if (laser) Destroy(laser.gameObject);
    }
}
