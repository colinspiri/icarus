using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayerGun : MonoBehaviour {
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletOriginObject;
    [Space]
    [SerializeField] private float bulletSpeed;
    [SerializeField] private FloatVariable heat;
    [SerializeField] private float heatDecreasePerShot;

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput() {
        // point the gun towards the mouse
        Vector2 lookPosition = GetLookPosition();
        LookAtPoint(lookPosition);

        // fire bullets
        if (InputManager.Instance.firePressed && heat.Value >= heatDecreasePerShot) {
            FireBullet();
            heat.Value -= heatDecreasePerShot;
        }
    }
    
    private Vector2 GetLookPosition()
    {
        Vector2 result = transform.right;
        
        result = new Vector2(InputManager.Instance.horizontalLookAxis, InputManager.Instance.verticalLookAxis);
        
        return result;
    }
    private void LookAtPoint(Vector3 point)
    {
        if (Time.timeScale > 0)
        {
            // Rotate the player to look at the mouse.
            var screenToWorldPoint = Camera.main.ScreenToWorldPoint(point);
            Vector2 lookDirection = screenToWorldPoint - transform.position;

            transform.right = lookDirection;
        }
    }

    private void FireBullet() {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.originObject = bulletOriginObject;
        
        bullet.MoveSpeed = bulletSpeed;
    }
}
