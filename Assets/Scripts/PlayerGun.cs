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
    [SerializeField] private GunConstants gunConstants;

    [Header("Variables")] 
    [SerializeField] private IntVariable currentAmmo;
    [SerializeField] private FloatVariable reloadProgress;

    [Header("Bullet Prefabs")] 
    [SerializeField] private GameObject bulletPrefabLowHeat;
    [SerializeField] private GameObject bulletPrefabMediumHeat;
    [SerializeField] private GameObject bulletPrefabHighHeat;
    
    [Header("Audio")]
    [SerializeField] private AudioClip fireSound;
    
    // state
    private bool _reloading;
    private float _fireCooldownProgress;

    private void Start() {
        currentAmmo.Value = gunConstants.maxAmmo;
        reloadProgress.Value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        UpdateReload();
        UpdateFireCooldown();
    }

    private void HandleInput() {
        // point the gun towards the mouse
        Vector2 lookPosition = GetLookPosition();
        LookAtPoint(lookPosition);

        // fire bullets
        float fireCooldown = 1.0f / gunConstants.CurrentFiringRate;
        if (InputManager.Instance.fireHeld && currentAmmo.Value > 0 && _fireCooldownProgress >= fireCooldown) {
            FireBullet();

            _fireCooldownProgress = 0;

            currentAmmo.Value -= gunConstants.CurrentAmmoCost;
            if (currentAmmo.Value < 0) currentAmmo.Value = 0;
            
            heat.Value -= heatConstants.CurrentHeatCostPerShot;
        }

        if (InputManager.Instance.reloadPressed && !_reloading) {
            StartReload();
        }
    }

    private void UpdateReload() {
        // if already reloading, add to progress
        if (_reloading) {
            reloadProgress.Value += Time.deltaTime;
            
            // finish reload
            if (reloadProgress >= gunConstants.reloadTime) {
                _reloading = false;
                currentAmmo.Value = gunConstants.maxAmmo;
            }
        }
        // check to start reload 
        else if (currentAmmo.Value <= 0) {
            StartReload();
        }
    }

    private void UpdateFireCooldown() {
        float fireCooldownTimeTier3 = 1.0f / gunConstants.firingRateTier3;
        if (_fireCooldownProgress < fireCooldownTimeTier3) {
            _fireCooldownProgress += Time.deltaTime;
        }
    }
    
    private void StartReload() {
        float missingPercent = (float)(gunConstants.maxAmmo - currentAmmo.Value) / gunConstants.maxAmmo;
        float heatCost = Mathf.Lerp(heatConstants.heatCostPerReloadClipFull, heatConstants.heatCostPerReloadClipEmpty,
            missingPercent);
        heat.Value -= heatCost;

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
