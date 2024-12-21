using System;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class PlayerMovement : MonoBehaviour {
    [Header("References")]
    public static PlayerMovement Instance;
    [SerializeField] private TurnCar turnCar;
    [SerializeField] private PlayerColorController colorController;
    [SerializeField] private PlayerInfo playerInfo;
    [SerializeField] private FloatVariable heat;
    [SerializeField] private HeatConstants heatConstants;
    [SerializeField] private AudioClip dashSFX;

    // public constants
    [Header("Movement")]
    [SerializeField] private MovementMode movementMode;
    private enum MovementMode { Linear, Acceleration }
    [SerializeField] private Vector2 speedInnerRadius;
    [SerializeField] private Vector2 speedOuterRadius;
    
    [Header("Linear Movement Mode")]
    [SerializeField] private float linearSpeed;
    [Space]
    [SerializeField] private bool linearSpeedScalesWithHeat;
    [SerializeField] private MinMaxFloat linearSpeedByHeat;

    [Space]
    [Header("Acceleration Movement Mode")] 
    [SerializeField] private float startingSpeed;
    [SerializeField] private float topSpeed;
    [Tooltip("Acceleration curve, determines speed (y) at given time spent accelerating (x)")]
    [SerializeField] private AnimationCurve accelerationCurve;
    [SerializeField] private float accelerationTime;
    [Space]
    [SerializeField] private bool accelerationTimeScalesWithHeat;
    [SerializeField] private MinMaxFloat accelerationTimeByHeat;

    [Space]
    [Header("Dash")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashDuration;
    [SerializeField] private float bulletReflectRadius;
    [SerializeField] private bool dashRequiresHeat;
    [SerializeField] private GameEvent dashStartedEvent;
    [SerializeField] private GameEvent dashEndedEvent;
    public event Action OnDashEnded;
    
    // state
    private Vector2 _timeAccelerating;
    private bool _canDash = true;
    [HideInInspector] public bool isDashing;

    private void Awake() {
        Instance = this;
    }

    void Update() {
        if (GameManager.Instance.GamePaused) return;

        if (!isDashing) {
            HandleInput();
        }
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
        
        // dash
        if (Input.GetKeyDown(KeyCode.Space) && (!dashRequiresHeat || heat.Value >= heatConstants.heatCostPerDash)) {
            Dash();
            heat.Value -= heatConstants.heatCostPerDash;
        }
    }

    private void MovePlayerLinearMode(Vector3 movement) {
        if (movement == Vector3.zero) return;

        var chosenSpeed = linearSpeedScalesWithHeat ? linearSpeedByHeat.LerpValue(heat.Value) : linearSpeed;

        Vector2 speed = new Vector2(chosenSpeed, chosenSpeed);
        speed = LimitSpeedNearBoundaries(movement, speed);
        
        // Debug.Log("speed = " + speed.x + ", " + speed.y);

        transform.Translate(new Vector3(movement.x * speed.x, movement.y * speed.y, 0) * Time.deltaTime);
    }

    private void MovePlayerAccelerationMode(Vector3 movement) {
        Vector2 speed = Vector2.zero;
        
        var chosenAccelerationTime = accelerationTimeScalesWithHeat ? accelerationTimeByHeat.LerpValue(heat.Value) : accelerationTime;

        // update time accelerating x
        if (movement.x == 0) {
            _timeAccelerating.x = 0;
        } 
        else if (movement.x > 0) {
            if (_timeAccelerating.x < chosenAccelerationTime) {
                if (_timeAccelerating.x < 0) {
                    _timeAccelerating.x = 0;
                }
                _timeAccelerating.x += Time.deltaTime;
            }
        }
        else if (movement.x < 0) {
            if (Mathf.Abs(_timeAccelerating.x) < chosenAccelerationTime) {
                if (_timeAccelerating.x > 0) {
                    _timeAccelerating.x = 0;
                }
                _timeAccelerating.x -= Time.deltaTime;
            }
        }
        // y 
        if (movement.y == 0) {
            _timeAccelerating.y = 0;
        } 
        else if (movement.y > 0) {
            if (_timeAccelerating.y < chosenAccelerationTime) {
                if (_timeAccelerating.y < 0) {
                    _timeAccelerating.y = 0;
                }
                _timeAccelerating.y += Time.deltaTime;
            }
        }
        else if (movement.y < 0) {
            if (Mathf.Abs(_timeAccelerating.y) < chosenAccelerationTime) {
                if (_timeAccelerating.y > 0) {
                    _timeAccelerating.y = 0;
                }
                _timeAccelerating.y -= Time.deltaTime;
            }
        }
        
        // determine speed based on time accelerating 
        Vector2 absoluteTimeAccelerating = new Vector2(Mathf.Abs(_timeAccelerating.x), MathF.Abs(_timeAccelerating.y));
        if (movement.x != 0) {
            if (absoluteTimeAccelerating.x > chosenAccelerationTime) {
                speed.x = topSpeed;
            }
            else {
                var curveInput = absoluteTimeAccelerating.x / chosenAccelerationTime;
                var curveValue = accelerationCurve.Evaluate(curveInput);
                speed.x = Mathf.Lerp(startingSpeed, topSpeed, curveValue);
            }
        }
        if (movement.y != 0) {
            if (absoluteTimeAccelerating.y > chosenAccelerationTime) {
                speed.y = topSpeed;
            }
            else {
                var curveInput = absoluteTimeAccelerating.y / chosenAccelerationTime;
                var curveValue = accelerationCurve.Evaluate(curveInput);
                speed.y = Mathf.Lerp(startingSpeed, topSpeed, curveValue);
            }
        }

        // limit speed 
        speed = LimitSpeedNearBoundaries(movement, speed);

        // translate 
        transform.Translate(new Vector3(movement.x * speed.x, movement.y * speed.y, 0) * Time.deltaTime);
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

    private void Dash() 
    {
        _canDash = false;
        isDashing = true;
        Vector2 dashDirection;
        Vector2 movementInput = new Vector2(InputManager.Instance.horizontalMoveAxis, InputManager.Instance.verticalMoveAxis);

        if (movementInput != Vector2.zero) dashDirection = movementInput.normalized;
        else dashDirection = playerInfo.gunFacingDirection;

        dashStartedEvent.Raise();

        Vector3 targetPosition = transform.position + (Vector3)dashDirection * dashDistance;
        targetPosition = ClampDashPosition(targetPosition);

        transform.DOMove(targetPosition, dashDuration).OnComplete(() => {
            isDashing = false;
            _canDash = true;
            dashEndedEvent.Raise();
            OnDashEnded?.Invoke();
        });
        
        colorController.Dash(dashDuration);

        ReflectBullets();
        AudioManager.Instance.Play(dashSFX, 1.0f);
    }

    private Vector3 ClampDashPosition(Vector3 position)
    {
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(position);
        viewportPoint.x = Mathf.Clamp(viewportPoint.x, 0.05f, 0.95f);
        viewportPoint.y = Mathf.Clamp(viewportPoint.y, 0.05f, 0.95f);
        return Camera.main.ViewportToWorldPoint(viewportPoint);
    }

    private void ReflectBullets()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bulletReflectRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Bullet"))
            {
                Bullet bullet = collider.GetComponent<Bullet>();
                if (bullet.fromPlayer || bullet.reflected) continue;
                bullet.Reflect();
            }
        }
    }
}