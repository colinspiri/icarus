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

    [Header("Ammo")] 
    [SerializeField] private IntVariable currentAmmo;
    [SerializeField] private IntReference maxAmmo;
    [SerializeField] private IntReference ammoCostLowHeat;
    [SerializeField] private IntReference ammoCostMediumHeat;
    [SerializeField] private IntReference ammoCostHighHeat;
    [Space]
    [SerializeField] private FloatReference reloadTime;
    [SerializeField] private FloatVariable reloadProgress;
    private int CurrentAmmoCost => heatConstants.CurrentHeatValue switch {
        HeatValue.Low => ammoCostLowHeat.Value,
        HeatValue.Medium => ammoCostMediumHeat.Value,
        HeatValue.High => ammoCostHighHeat.Value,
        _ => ammoCostLowHeat.Value,
    };

    [Header("Bullet Prefabs")] 
    [SerializeField] private GameObject bulletPrefabLowHeat;
    [SerializeField] private GameObject bulletPrefabMediumHeat;
    [SerializeField] private GameObject bulletPrefabHighHeat;
    
    [Header("Audio")]
    [SerializeField] private AudioClip fireSound;
    
    // state
    private bool _reloading;

    private void Start() {
        currentAmmo.Value = maxAmmo.Value;
        reloadProgress.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateReload();
    }

    private void HandleInput() {
        // point the gun towards the mouse
        Vector2 lookPosition = GetLookPosition();
        LookAtPoint(lookPosition);

        // fire bullets
        if (InputManager.Instance.firePressed && currentAmmo.Value > 0) {
            FireBullet();

            currentAmmo.Value -= CurrentAmmoCost;
            if (currentAmmo.Value < 0) currentAmmo.Value = 0;
            
            heat.Value -= heatConstants.CurrentHeatCostPerShot;
        }

        if (InputManager.Instance.reloadPressed) {
            StartReload();
        }
    }

    private void UpdateReload() {
        // if already reloading, add to progress
        if (_reloading) {
            reloadProgress.Value += Time.deltaTime;
            
            // finish reload
            if (reloadProgress >= reloadTime.Value) {
                _reloading = false;
                currentAmmo.Value = maxAmmo.Value;
            }
        }
        // check to start reload 
        else if (currentAmmo.Value <= 0) {
            StartReload();
        }
    }

    private void StartReload() {
        currentAmmo.Value = 0;
        _reloading = true;
        reloadProgress.Value = 0;
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
