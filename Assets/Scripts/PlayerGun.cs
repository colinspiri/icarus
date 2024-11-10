using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerGun : MonoBehaviour {
    [Header("References")] 
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletOriginObject;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private FloatVariable heat;
    [SerializeField] private HeatConstants heatConstants;
    /*[SerializeField] private float mediumHeatThreshold; 
    [SerializeField] private float highHeatThreshold; 
    [Space]
    [SerializeField] private float heatDecreasePerShotLowHeat; 
    [SerializeField] private float heatDecreasePerShotMediumHeat; 
    [SerializeField] private float heatDecreasePerShotHighHeat;*/

    [Header("Bullet Prefabs")] 
    [SerializeField] private GameObject bulletPrefabLowHeat;
    [SerializeField] private GameObject bulletPrefabMediumHeat;
    [SerializeField] private GameObject bulletPrefabHighHeat;
    
    [Header("Audio")]
    [SerializeField] private AudioClip fireSound;

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

            heat.Value -= heatConstants.CurrentHeatCostPerShot;
        }
    }

    private Vector2 GetLookPosition()
    {
        Vector2 result = transform.right;
        playerInfo.gunFacingDirection = result;
        
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
        GameObject bulletPrefab = heatConstants.CurrentHeatValue switch {
            HeatValue.Low => bulletPrefabLowHeat,
            HeatValue.Medium => bulletPrefabMediumHeat,
            HeatValue.High => bulletPrefabHighHeat,
            _ => bulletPrefabLowHeat
        };
        
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.originObject = bulletOriginObject;
        
        AudioManager.Instance.Play(fireSound, .5f);  
    }
}
