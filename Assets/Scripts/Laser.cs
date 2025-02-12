using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float laserDuration = 3.0f;
    [SerializeField] private float laserDamageDuration = 0.5f;
    [SerializeField] private float laserDamage = 0.3f;

    [SerializeField] private GameEvent laserDestroyed;

    private bool _canDamagePlayer = false;
    private bool _playerIsInLaser = false;

    private void Start()
    {
        StartCoroutine(FireLaser());
        _canDamagePlayer = false;
    }

    private void Update()
    {
        if (_playerIsInLaser && _canDamagePlayer) 
            PlayerMovement.Instance.gameObject.GetComponent<Health>().TakeDamage(laserDamage);
    }

    private IEnumerator FireLaser()
    {
        float elapsedTime = 0f;
        float startOpacity = GetComponent<SpriteRenderer>().color.a; // this should be 0
        Color color = GetComponent<SpriteRenderer>().color;

        while (elapsedTime < laserDuration + laserDamageDuration)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > laserDuration && elapsedTime < laserDuration + laserDamageDuration) _canDamagePlayer = true;
            else _canDamagePlayer = false;

            float newOpacity = Mathf.Lerp(startOpacity, 1f, elapsedTime / laserDuration);
            Color newColor = new(color.r, color.g, color.b, newOpacity);
            GetComponent<SpriteRenderer>().color = newColor;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_canDamagePlayer) collision.GetComponent<Health>().TakeDamage(laserDamage);
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
