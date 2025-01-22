using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : EnemyGun
{
    [Header("Machine Gun")]
    [SerializeField] private BurstMode burstMode = BurstMode.Straight;
    [SerializeField] private float burstFiringDelay;
    [SerializeField] private float burstBulletQuantity;

    [Space]
    [SerializeField] private float arcAngle = 60f;

    private enum BurstMode
    {
        Straight,
        Arc,
        Random
    };

    protected override void Update()
    {
        LookAtPoint(PlayerMovement.Instance.transform.position);

        if (_currentFiringCooldown <= 0 && _canFire && IsFacingPlayer())
        {
            if (burstMode == BurstMode.Straight || burstMode == BurstMode.Random) StartCoroutine(StraightBurst());
            else if (burstMode == BurstMode.Arc) StartCoroutine(ArcBurst());
        }
        else _currentFiringCooldown -= Time.deltaTime;
    }

    private IEnumerator StraightBurst()
    {
        _canFire = false;

        for (int i = 0; i < burstBulletQuantity; i++)
        {
            Fire();
            yield return new WaitForSeconds(burstFiringDelay);
        }

        StartFiringCooldown();
        _canFire = true;
    }

    private IEnumerator ArcBurst()
    {
        _canFire = false;

        float halfArcAngle = arcAngle / 2;
        float bulletRotationAngle = arcAngle / burstBulletQuantity;

        for (float currentBulletRotaion = -halfArcAngle; currentBulletRotaion < halfArcAngle; currentBulletRotaion += bulletRotationAngle)
        {
            FireAtAngle(currentBulletRotaion);
            yield return new WaitForSeconds(burstFiringDelay);
        }

        StartFiringCooldown();
        _canFire = true;
    }

    private void FireAtAngle(float angle)
    {
        Quaternion additionalRotation = Quaternion.Euler(0, 0, angle);
        Quaternion rotation = bulletSpawnPoint.rotation * additionalRotation;

        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, rotation).GetComponent<Bullet>();
        bullet.originObject = bulletOriginObject;

        AudioManager.Instance.Play(enemyShoot, 0.1f);
    }
}
