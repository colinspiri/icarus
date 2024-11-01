using System;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Instance;
    // components
    [SerializeField] private TurnCar turnCar;
    
    // public constants
    private enum MovementMode { Linear, Acceleration }
    [SerializeField] private MovementMode movementMode;
    [SerializeField] private Vector2 speedInnerRadius;
    [SerializeField] private Vector2 speedOuterRadius;
    
    [Header("Linear Movement Mode")]
    [SerializeField] private float linearSpeed; // 6.5

    [Header("Acceleration Movement Mode")] 
    [SerializeField] private float startingSpeed;
    [SerializeField] private float topSpeed;
    [SerializeField] private float accelerationTime;
    [SerializeField] private AnimationCurve accelerationCurve;
    
    [Space]
    [Header("Heat")]
    [SerializeField] private FloatVariable heat;
    [SerializeField] private float heatIncreaseSpeed;
    
    // state
    
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        heat.Value = 0;
    }

    void Update()
    {
        HandleInput();

        heat.Value += Time.deltaTime * heatIncreaseSpeed;
        if (heat.Value > 1) heat.Value = 1;
    }

    private void HandleInput()
    {
        // move player
        Vector3 movementVector = new Vector3(InputManager.Instance.horizontalMoveAxis, InputManager.Instance.verticalMoveAxis, 0);
        if (movementMode == MovementMode.Linear) {
            MovePlayerLinearMode(movementVector);
        }
        else if (movementMode == MovementMode.Acceleration) {
            MovePlayerAccelerationMode(movementVector);
        }

        // set turn direction
        int turnDirection = (movementVector.y == 0) ? 0 : (int)(movementVector.y / Mathf.Abs(movementVector.y));
        turnCar.TurnDirection = turnDirection;
    }

    private void MovePlayerLinearMode(Vector3 movement) {
        if (movement == Vector3.zero) return;
        
        Vector2 speed = new Vector2(linearSpeed, linearSpeed);
        speed = LimitSpeedNearBoundaries(movement, speed);
        
        // Debug.Log("speed = " + speed.x + ", " + speed.y);

        transform.Translate(new Vector3(movement.x * speed.x, movement.y * speed.y, 0) * Time.deltaTime);
    }

    private void MovePlayerAccelerationMode(Vector3 movement) {
        
    }

    // returns limited speed
    private Vector2 LimitSpeedNearBoundaries(Vector3 movementDirection, Vector2 speed) {
        float distanceValueX = Mathf.InverseLerp(speedInnerRadius.x, speedOuterRadius.x, Mathf.Abs(transform.position.x));
        float reducedSpeedX = Mathf.Lerp(speed.x, 0, distanceValueX);
        bool movingTowardsCenterX =
            (transform.position.x < 0 && movementDirection.x > 0) || (transform.position.x > 0 && movementDirection.x < 0);
        speed.x = movingTowardsCenterX ? speed.x : reducedSpeedX;
        
        float distanceValueY = Mathf.InverseLerp(speedInnerRadius.y, speedOuterRadius.y, Mathf.Abs(transform.position.y));
        float reducedSpeedY = Mathf.Lerp(speed.y, 0, distanceValueY);
        bool movingTowardsCenterY =
            (transform.position.y < 0 && movementDirection.y > 0) || (transform.position.y > 0 && movementDirection.y < 0);
        speed.y = movingTowardsCenterY ? speed.y : reducedSpeedY;

        return speed;
    }
}