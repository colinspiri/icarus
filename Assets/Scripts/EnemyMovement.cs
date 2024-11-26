using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class EnemyMovement : MonoBehaviour {
    [SerializeField] private GameEvent movedToOrigin;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float originMoveSpeed;
    
    private enum State { MoveToOrigin, MoveRandomly }
    private State _currentState;
    private Vector3 _anchorPosition;
    
    // move randomly 
    [SerializeField] private float randomMoveRadius;
    [SerializeField] private float changeDirectionTime;
    [SerializeField] private Vector2 onscreenSize;
    [Tooltip("1 when aligned, 0 when opposite")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float towardsAnchorAlignmentFactor = 0.5f;
    [Tooltip("1 when aligned, 0 when opposite")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float currentDirectionAlignmentFactor = 0.5f;
    
    // private state 
    private Vector3 _currentMoveDirection;
    private float _changeDirectionTimer;

    // Start is called before the first frame update
    void Start() {
        _currentState = State.MoveToOrigin;
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState == State.MoveToOrigin) UpdateMoveToOrigin();
        else UpdateMoveRandomly();
    }

    private void UpdateMoveToOrigin() {
        Vector3 directionToOrigin = _anchorPosition - transform.position;
        float distanceToOrigin = directionToOrigin.magnitude;
        directionToOrigin.Normalize();
        transform.Translate(directionToOrigin * (Time.deltaTime * originMoveSpeed));

        if (distanceToOrigin < 1) {
            _currentState = State.MoveRandomly;
            movedToOrigin.Raise();
        }
    }

    private void UpdateMoveRandomly() {
        // maybe try having enemies accelerate forwards/backwards but move sharply left/right
        // to look like theyre racing 
        
        // pick a new direction
        if (_changeDirectionTimer <= 0) {
            ChooseNewRandomMoveDirection();
            _changeDirectionTimer = changeDirectionTime;
        }
        // move in direction
        else {
            transform.Translate(_currentMoveDirection * (Time.deltaTime * moveSpeed));
            _changeDirectionTimer -= Time.deltaTime;
        }

    }

    private void ChooseNewRandomMoveDirection() {
        Vector3 toAnchor = _anchorPosition - transform.position;
        
        Vector3 newDirection;
        bool newDirectionAccepted = true;
        do {
            newDirection = Random.onUnitSphere;
            
            // if too far from anchor point, must be in direction of anchor point 
            bool farFromAnchor = toAnchor != Vector3.zero && toAnchor.magnitude >= randomMoveRadius;
            if (farFromAnchor || IsOffscreen()) {
                var dot = Vector3.Dot(newDirection.normalized, toAnchor.normalized);
                var f = (dot + 1) / 2.0f; // 1 when aligned, 0 when opposite 
                newDirectionAccepted = f >= towardsAnchorAlignmentFactor;
            }
            
            // must be similar to current direction
            else if (_currentMoveDirection != Vector3.zero) {
                var dot = Vector3.Dot(newDirection.normalized, _currentMoveDirection.normalized);
                var f = (dot + 1) / 2.0f; // 1 when aligned, 0 when opposite 
                newDirectionAccepted = f >= currentDirectionAlignmentFactor;
            }
        } while (!newDirectionAccepted);
        
        _currentMoveDirection = newDirection;
    }

    public void SetAnchor(Vector3 position) {
        _anchorPosition = position;
    }

    private bool IsOffscreen() {
        var pos = transform.position;
        bool offscreen = pos.x < -onscreenSize.x || pos.x > onscreenSize.x || pos.y < -onscreenSize.y ||
                         pos.y > onscreenSize.y;
        // if (offscreen) {
        //     Debug.Log(name + " is offscreen");
        // }
        return offscreen;
    }
}
