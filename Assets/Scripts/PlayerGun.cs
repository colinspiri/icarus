using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour {
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;

    [SerializeField] private float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
        if (InputManager.Instance.firePressed) {
            FireBullet();
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
        // bullet.transform.rotation = transform.rotation;
        bullet.MoveSpeed = bulletSpeed;
    }
}
