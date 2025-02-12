using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using DG.Tweening;

public class Laser : MonoBehaviour
{
    [SerializeField] private LaserSO laserSO;
    [SerializeField] private GameEvent laserDestroyed;
    [SerializeField] private float endingOpacity = 1f;

    private bool _canDamagePlayer = false;
    private bool _playerIsInLaser = false;

    private Color _originalColor;
    private SpriteRenderer _sprite;

    private void Start()
    {
        //StartCoroutine(FireLaser());
        _sprite = GetComponent<SpriteRenderer>();
        _originalColor = _sprite.color;
        FireLaser();
        _canDamagePlayer = false;
    }

    private void Update()
    {
        if (_playerIsInLaser && _canDamagePlayer) 
            PlayerMovement.Instance.gameObject.GetComponent<Health>().TakeDamage(laserSO.laserDamage);
    }

    private void FireLaser() {
        _sprite.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, endingOpacity);
        Invoke("StartLaserDamageCoroutine", laserSO.laserDuration);
        /*_sprite.DOFade(endingOpacity, laserSO.laserDuration).SetEase(Ease.InExpo)
            .OnComplete(() => {
                _sprite.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1);
                StartCoroutine(LaserDamage());
            });*/
    }

    private void StartLaserDamageCoroutine() {
        StartCoroutine(LaserDamage());
    }

    private IEnumerator LaserDamage()
    {
        _sprite.color = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 1);
        _canDamagePlayer = true;
        yield return new WaitForSeconds(laserSO.laserDamageDuration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_canDamagePlayer) collision.GetComponent<Health>().TakeDamage(laserSO.laserDamage);
            _playerIsInLaser = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) _playerIsInLaser = false;
    }

    private void OnDestroy()
    {
        laserDestroyed.Raise();
    }
}
