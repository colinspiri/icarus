using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectMovement : MonoBehaviour
{
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;

    private float speed;

    private void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (IsOffScreen()) Destroy(gameObject);
    }

    private bool IsOffScreen()
    {
        Vector3 screenLeftBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.5f, Camera.main.nearClipPlane));

        if (transform.position.x < screenLeftBounds.x - 1) return true;
        return false;
    }
}
