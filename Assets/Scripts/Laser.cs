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

    private bool _canDamagePlayer = false;
    private bool _playerIsInLaser = false;

    private void Start()
    {
        //StartCoroutine(FireLaser());
        FireLaser();
        _canDamagePlayer = false;
    }

    private void Update()
    {
        if (_playerIsInLaser && _canDamagePlayer) 
            PlayerMovement.Instance.gameObject.GetComponent<Health>().TakeDamage(laserSO.laserDamage);
    }

    private void FireLaser()
    {
        GetComponent<SpriteRenderer>().DOFade(1f, laserSO.laserDuration).SetEase(Ease.InExpo)
            .OnComplete(() => StartCoroutine(LaserDamage()));
    }

    private IEnumerator LaserDamage()
    {
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
