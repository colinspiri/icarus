using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : EnemyGun
{
    [SerializeField] private float burstFiringDelay;
    [SerializeField] private float burstBulletQuantity;

    protected override void Update()
    {
        LookAtPoint(PlayerMovement.Instance.transform.position);

        if (_currentFiringCooldown <= 0 && _canFire && IsFacingPlayer()) StartCoroutine(Burst());
        else _currentFiringCooldown -= Time.deltaTime;
    }

    private IEnumerator Burst()
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
}
