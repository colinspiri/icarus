using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour
{
    private Transform _target;
    private Rigidbody2D _rb;
    public float speed = 5f;
    public float rotateSpeed = 200f;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)_target.position - _rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        _rb.angularVelocity = -rotateAmount * rotateSpeed;

        _rb.velocity = transform.right * speed;
    }
}
