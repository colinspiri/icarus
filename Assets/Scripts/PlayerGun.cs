using System.Collections;
using System.Collections.Generic;
using System.Resources;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerGun : MonoBehaviour {
    [Header("Components")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletOriginObject;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private PlayerInfo playerInfo;

    [Header("Heat")]
    [SerializeField] private FloatVariable heat;
    [SerializeField] private float mediumHeatThreshold; // 0.33
    [SerializeField] private float highHeatThreshold; // 0.66
    [Space]
    [SerializeField] private float heatDecreasePerShotLowHeat; 
    [SerializeField] private float heatDecreasePerShotMediumHeat; 
    [SerializeField] private float heatDecreasePerShotHighHeat;

    [Header("Bullet Prefabs")] 
    [SerializeField] private GameObject bulletPrefabLowHeat;
    [SerializeField] private GameObject bulletPrefabMediumHeat;
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
            
            var heatDecrease = heatDecreasePerShotLowHeat;
            if (heat.Value >= highHeatThreshold) heatDecrease = heatDecreasePerShotHighHeat;
            else if (heat.Value >= mediumHeatThreshold) heatDecrease = heatDecreasePerShotMediumHeat;
            
            heat.Value -= heatDecrease;
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
        GameObject chosenBulletPrefab;
        if (heat.Value >= highHeatThreshold) chosenBulletPrefab = bulletPrefabHighHeat;
        else if (heat.Value >= mediumHeatThreshold) chosenBulletPrefab = bulletPrefabMediumHeat;
        else chosenBulletPrefab = bulletPrefabLowHeat;
        
        Bullet bullet = Instantiate(chosenBulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        bullet.originObject = bulletOriginObject;
        
        AudioManager.Instance.Play(fireSound, .5f);  
    }
}
