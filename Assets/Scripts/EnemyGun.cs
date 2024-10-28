using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour {
    [SerializeField] private float rotationSpeed;

    [SerializeField] private MinMaxFloat randomFiringCooldown;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletOriginObject;
    [Space] 
    [SerializeField] private float staticBulletSpeed;
    [SerializeField] private bool useRandomBulletSpeed;
    [SerializeField] private MinMaxFloat randomBulletSpeed;

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
            FireBullet();
            _currentFiringCooldown = randomFiringCooldown.RandomValue;
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
    
    private void FireBullet() {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.originObject = bulletOriginObject;
        
        bullet.MoveSpeed = useRandomBulletSpeed ? randomBulletSpeed.RandomValue : staticBulletSpeed;
    }
}
