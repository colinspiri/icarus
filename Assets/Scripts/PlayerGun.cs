using System.Collections;
using System.Collections.Generic;
using System.Resources;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerGun : MonoBehaviour {
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletOriginObject;
    [Space]
    [SerializeField] private FloatVariable heat;
    // [SerializeField] private float heatDecreasePerShot;
    [SerializeField] private AudioClip fireSound;

    [Header("Bullet Prefabs")] 
    [SerializeField] private GameObject bulletPrefabLowHeat;
    [SerializeField] private float mediumHeatValue;
    [SerializeField] private GameObject bulletPrefabMediumHeat;
    [SerializeField] private float highHeatValue;
    [SerializeField] private GameObject bulletPrefabHighHeat;

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
        GameObject chosenBulletPrefab;
        if (heat.Value >= highHeatValue) chosenBulletPrefab = bulletPrefabHighHeat;
        else if (heat.Value >= mediumHeatValue) chosenBulletPrefab = bulletPrefabMediumHeat;
        else chosenBulletPrefab = bulletPrefabLowHeat;
        
        Bullet bullet = Instantiate(chosenBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.originObject = bulletOriginObject;
        
        AudioManager.Instance.Play(fireSound, .5f);  
    }
}
