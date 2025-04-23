using ScriptableObjectArchitecture;
using UnityEngine;

public class PlayerGun : MonoBehaviour {
    [Header("References")] 
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletOriginObject;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private FloatVariable heat;
    [SerializeField] private HeatConstants heatConstants;
    [SerializeField] private EquippedGun equippedGun;
    private GunConstants CurrentGun => equippedGun.CurrentGun;

    [Header("Constants")] 
    [SerializeField] private IntReference maxAmmo; 
    [SerializeField] private FloatReference reloadTime; 
    
    [Header("Variables")] 
    [SerializeField] private IntVariable currentAmmo;
    [SerializeField] private FloatVariable reloadProgress;

    [Header("Audio")]
    [SerializeField] private SoundProfile reloadSound;

    
    // state
    private bool _reloading;
    private float _fireCooldownProgress;
    private bool _previousFireHeld;

    public LaserState laserState;
    public enum LaserState
    {
        Charging,
        Fired
    }

    private void Start() {
        currentAmmo.Value = maxAmmo.Value;
        reloadProgress.Value = 0;
        laserState = LaserState.Fired;
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.Instance.GamePaused) return;
        HandleInput();
        UpdateReload();
        UpdateFireCooldown();

        // Detects if player releases the fire button while using the laser gun
        // When released, we can then start the cooldown for the laser gun
        if (CurrentGun is LaserGunConstants && _previousFireHeld && !InputManager.Instance.fireHeld)
        {
            SetLaserFired();
        }

        _previousFireHeld = InputManager.Instance.fireHeld;
    }

    private void HandleInput() {
        // point the gun towards the mouse
        Vector2 lookPosition = GetLookPosition();
        LookAtPoint(lookPosition);

        // fire bullets
        float fireCooldown = 1.0f / CurrentGun.CurrentFiringRate;
        if (CurrentGun is LaserGunConstants && InputManager.Instance.fireHeld && currentAmmo.Value > 0
            && _fireCooldownProgress >= fireCooldown && laserState == LaserState.Fired){
            FireLaser();
            _fireCooldownProgress = 0;

            currentAmmo.Value -= CurrentGun.CurrentAmmoCost;
            if (currentAmmo.Value < 0) currentAmmo.Value = 0;

            heatConstants.CalculateCurrentHeat(-heatConstants.CurrentHeatCostPerShot);

            /* setting the laser state to charging will keep the laser from continuing to
               fire and prevent the cooldown from starting until the laser has been fired */
            laserState = LaserState.Charging;
        }
        else if (CurrentGun is not LaserGunConstants && InputManager.Instance.fireHeld && currentAmmo.Value > 0 
            && _fireCooldownProgress >= fireCooldown){
            FireBullet();

            _fireCooldownProgress = 0;

            currentAmmo.Value -= CurrentGun.CurrentAmmoCost;
            if (currentAmmo.Value < 0) currentAmmo.Value = 0;

            heatConstants.CalculateCurrentHeat(-heatConstants.CurrentHeatCostPerShot);
            //heat.Value -= heatConstants.CurrentHeatCostPerShot;
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
            if (reloadProgress >= reloadTime.Value) {
                _reloading = false;
                currentAmmo.Value = maxAmmo.Value;
            }
        }
        // check to start reload 
        else if (currentAmmo.Value <= 0 && laserState == LaserState.Fired) {
            StartReload();
        }
    }

    private void UpdateFireCooldown() {
        if (CurrentGun is not LaserGunConstants || laserState == LaserState.Fired)
        {
            float minFiringRate = Mathf.Min(CurrentGun.firingRateTier1, CurrentGun.firingRateTier2,
                CurrentGun.firingRateTier3);
            float maxFireCooldown = 1.0f / minFiringRate;
            if (_fireCooldownProgress < maxFireCooldown) {
                _fireCooldownProgress += Time.deltaTime;
            }
        }
    }
    
    private void StartReload() {
        float missingPercent = (float)(maxAmmo.Value - currentAmmo.Value) / maxAmmo.Value;
        float heatCost = Mathf.Lerp(heatConstants.heatCostPerReloadClipFull, heatConstants.heatCostPerReloadClipEmpty,
            missingPercent);
        heatConstants.CalculateCurrentHeat(-heatCost);
        //heat.Value -= heatCost;

        currentAmmo.Value = 0;
        _reloading = true;
        reloadProgress.Value = 0;
        reloadSound.PlaySFX();
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
        GameObject bulletPrefab = CurrentGun.CurrentBulletPrefab;
        
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation).GetComponent<Bullet>();
        if (bullet != null) {
            bullet.originObject = bulletOriginObject;
        }
        
        // add spread
        if (CurrentGun.CurrentSpread > 0) {
            float randomRotation = Random.Range(-0.5f * CurrentGun.CurrentSpread, 0.5f * CurrentGun.CurrentSpread);
            bullet.transform.Rotate(0, 0, randomRotation);
        }

        if (CurrentGun.CurrentFireSFX != null) {
            CurrentGun.CurrentFireSFX.PlaySFX();
        }
    }

    private void FireLaser()
    {
        GameObject laserPrefab = CurrentGun.CurrentBulletPrefab;
        var spriteRenderer = laserPrefab.GetComponent<SpriteRenderer>();
        var spawnPoint = bulletSpawnPoint.position + bulletSpawnPoint.right * spriteRenderer.bounds.size.x / 2;

        Instantiate(laserPrefab, spawnPoint, bulletSpawnPoint.rotation, gameObject.transform);

        //CurrentGun.CurrentFireSFX.PlaySFX();
    }

    public void SetLaserFired() => laserState = LaserState.Fired;
}
