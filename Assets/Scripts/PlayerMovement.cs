using System;
using ScriptableObjectArchitecture;
using UnityEngine;

// This class controls player movement
// Script is based from Coursera Game Design and Development 1: 2D Shooter project from MSU
public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Instance;

    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 speedInnerRadius;
    [SerializeField] private Vector2 speedOuterRadius;
    [SerializeField] private float anchorPointSpeed;
    [Space]
    [SerializeField] private FloatVariable heat;
    [SerializeField] private float heatIncreaseSpeed;

    private Vector3 _anchorPoint;
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        _anchorPoint = transform.position;
    }

    void Update()
    {
        HandleInput();
        MoveAnchorPoint();

        heat.Value += Time.deltaTime * heatIncreaseSpeed;
        if (heat.Value > 1) heat.Value = 1;
    }

    private void HandleInput()
    {
        Vector3 movementVector = new Vector3(InputManager.Instance.horizontalMoveAxis, InputManager.Instance.verticalMoveAxis, 0);
        MovePlayer(movementVector);
    }

    private void MovePlayer(Vector3 movement) {
        if (movement == Vector3.zero) return;
        
        float innerRadius = Mathf.Lerp(speedInnerRadius.x, speedInnerRadius.y, heat);
        float outerRadius = Mathf.Lerp(speedOuterRadius.x, speedOuterRadius.y, heat);
        
        Vector3 playerToAnchor = _anchorPoint - transform.position;
        float distanceFromAnchor = playerToAnchor.magnitude;

        float distanceValue = Mathf.InverseLerp(innerRadius, outerRadius, distanceFromAnchor);
        float reducedSpeed = Mathf.Lerp(moveSpeed, 0, distanceValue);

        float dot = Vector3.Dot(movement.normalized, playerToAnchor.normalized);
        float f = (dot + 1) / 2.0f; // 1 when moving to center, 0 when moving away
        float speed = Mathf.Lerp(reducedSpeed, moveSpeed, f);
        
        Debug.Log("distanceFromCenter = " + distanceFromAnchor + ", " + "dot f = " + f + ", speed = " + speed);

        transform.Translate(movement * (Time.deltaTime * speed));
    }

    private void MoveAnchorPoint() {
        Vector3 anchorToPlayer = transform.position - _anchorPoint;
        _anchorPoint += anchorToPlayer * (anchorPointSpeed * Time.deltaTime);
    }
}