using System;
using ScriptableObjectArchitecture;
using UnityEngine;

// This class controls player movement
// Script is based from Coursera Game Design and Development 1: 2D Shooter project from MSU
public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Instance;
    // components
    [SerializeField] private TurnCar turnCar;
    [Space]
    [SerializeField] private float minMoveSpeed;
    [SerializeField] private float maxMoveSpeed;
    private float moveSpeed => Mathf.Lerp(minMoveSpeed, maxMoveSpeed, heat.Value);
    
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
        // MoveAnchorPoint();

        heat.Value += Time.deltaTime * heatIncreaseSpeed;
        if (heat.Value > 1) heat.Value = 1;
    }

    private void HandleInput()
    {
        // move player
        Vector3 movementVector = new Vector3(InputManager.Instance.horizontalMoveAxis, InputManager.Instance.verticalMoveAxis, 0);
        MovePlayer(movementVector);

        // set turn direction
        int turnDirection = (movementVector.y == 0) ? 0 : (int)(movementVector.y / Mathf.Abs(movementVector.y));
        turnCar.TurnDirection = turnDirection;
    }

    private void MovePlayer(Vector3 movement) {
        if (movement == Vector3.zero) return;
        
        Vector2 speed = new Vector2(moveSpeed, moveSpeed);

        float distanceValueX = Mathf.InverseLerp(speedInnerRadius.x, speedOuterRadius.x, Mathf.Abs(transform.position.x));
        float reducedSpeedX = Mathf.Lerp(moveSpeed, 0, distanceValueX);
        bool movingTowardsCenterX =
            (transform.position.x < 0 && movement.x > 0) || (transform.position.x > 0 && movement.x < 0);
        speed.x = movingTowardsCenterX ? moveSpeed : reducedSpeedX;
        
        float distanceValueY = Mathf.InverseLerp(speedInnerRadius.y, speedOuterRadius.y, Mathf.Abs(transform.position.y));
        float reducedSpeedY = Mathf.Lerp(moveSpeed, 0, distanceValueY);
        bool movingTowardsCenterY =
            (transform.position.y < 0 && movement.y > 0) || (transform.position.y > 0 && movement.y < 0);
        speed.y = movingTowardsCenterY ? moveSpeed : reducedSpeedY;
        
        // Debug.Log("speed = " + speed.x + ", " + speed.y);

        transform.Translate(new Vector3(movement.x * speed.x, movement.y * speed.y, 0) * Time.deltaTime);
    }

    private void MoveAnchorPoint() {
        Vector3 anchorToPlayer = transform.position - _anchorPoint;
        _anchorPoint += anchorToPlayer * (anchorPointSpeed * Time.deltaTime);
    }
}