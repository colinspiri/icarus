using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ScriptableObjectArchitecture;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField] private LaserGunConstants laserGunConstants;
    [SerializeField] private HeatConstants heatConstants;
    [SerializeField] private GameEvent playerLaserFired;
    [SerializeField] private IntVariable extraAmmoCost;
    [SerializeField] private IntVariable currentAmmo;

    private SpriteRenderer _spriteRenderer;
    private float _laserDuration;
    private float finalLaserDamage;
    private bool _canDamage;

    [Header("Damage")]
    [SerializeField] private float damage = 1f;
    [SerializeField] private bool useValueFromGunConstants;
    [SerializeField] private GunConstants gunConstants;
    [SerializeField] private HeatValue tier;
    private float Damage => useValueFromGunConstants ? gunConstants.DamageFromTier(tier) : damage;

    void Start()
    {
        _canDamage = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().enabled = false;
        // start with laser transparent and make opaque once the laser is officially fired
        Color color = new(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 0.1f);
        transform.GetComponent<SpriteRenderer>().color = color;
        StartCoroutine(ChargeLaser());
        extraAmmoCost.Value = 0;
    }

    private IEnumerator ChargeLaser()
    {
        _laserDuration = 0f;
        float initialDamage = laserGunConstants.CurrentDamage;
        finalLaserDamage = initialDamage;
        float elapsedTime = 0f;

        while (InputManager.Instance.fireHeld && finalLaserDamage < laserGunConstants.maxDamage)
        {
            _laserDuration += Time.deltaTime;
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 1f)
            {
                extraAmmoCost.Value++;
                elapsedTime = 0f;
            }

            finalLaserDamage = initialDamage + (laserGunConstants.laserDamagePerSecond * _laserDuration);
            yield return null;
        }

        FireLaser();
        int ammoCost = laserGunConstants.CurrentAmmoCost + extraAmmoCost.Value;
        currentAmmo.Value -= ammoCost;
        if (currentAmmo.Value < 0) currentAmmo.Value = 0;
        extraAmmoCost.Value = 0;
    }

    private void FireLaser()
    {
        _canDamage = true;
        GetComponent<Collider2D>().enabled = true;
        _spriteRenderer.DOFade(1f, 0.1f).OnComplete(()=> Destroy(gameObject));
        playerLaserFired.Raise();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _canDamage)
        {
            collision.GetComponent<Health>().TakeDamage(finalLaserDamage);
            heatConstants.CalculateCurrentHeat(heatConstants.heatGainPerDamage * finalLaserDamage);
        }
    }
}
