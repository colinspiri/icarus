using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float LifeTime { get; set; } = 60f;
    public float MoveSpeed { get; set; } = 1f;

    private Vector2 _spawnPoint;
    private float _lifeTimer;

    private void Start() {
        _spawnPoint = transform.position;
        _lifeTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(_lifeTimer > LifeTime) Destroy(gameObject);
        _lifeTimer += Time.deltaTime;
        Move();
    }

    private void Move() {
        transform.Translate(transform.right * (Time.deltaTime * MoveSpeed), Space.World);
    }
}
