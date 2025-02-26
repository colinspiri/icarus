using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjectArchitecture;
using UnityEngine;
using UnityEngine.Serialization;


public class DeathFromAbove : MonoBehaviour
{
    [Header("Components")]
    public GameObject originObject;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Speed")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float moveDuration = 2f;

    [Header("Damage")]
    [SerializeField] private float damage = 1f;

    [Header("Audio")]
    [SerializeField] private AudioClip fireSFX;

    private Renderer objectRenderer;
    public Color newColor = Color.white;
    private bool isMoving;
    private bool hasFired;
    public bool alwaysOn;
    public bool freezeOnHit;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        objectRenderer = GetComponent<Renderer>();
        if (alwaysOn == false)
        {
            Invoke("StopMoving", moveDuration);
        }
        else
        {
            Invoke("ConstantFire", 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerMovement.Instance.transform.position, speed * Time.deltaTime);
        }
    }

    private void StopMoving()
    {
        isMoving = false;
        Invoke("Fire", 0.1f);

    }

    private void Fire()
    {
        objectRenderer.material.color = newColor;
        hasFired = true;
        //AudioManager.Instance.Play(fireSFX, 0.5f);
        Invoke("Despawn", 0.1f);
    }

    private void ConstantFire()
    {
        objectRenderer.material.color = newColor;
        hasFired = true;
        //AudioManager.Instance.Play(fireSFX, 0.5f);
        Invoke("Despawn", moveDuration);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject == null) return;

        if (hasFired && other.CompareTag("Player"))
        {
            var health = other.GetComponent<Health>();
            health.TakeDamage(damage);
            if(freezeOnHit == true)
            {
                //var speed = other.GetComponent<PlayerMovement>();
                //speed = 0f;
            }
        }
        if(hasFired == true && other.CompareTag("Enemy"))
        {
            var enemyHealth = other.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(10f);
               
        }
    }

    private void Despawn()
    {
        Destroy(gameObject);
    }
}
