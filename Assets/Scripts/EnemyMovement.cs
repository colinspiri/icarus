using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    
    private enum State { MoveToOrigin, MoveRandomly }
    private State _currentState;
    private Vector3 _anchorPosition;
    
    // move randomly 
    [SerializeField] private float randomMoveRadius;
    [SerializeField] private float changeDirectionTime;
    private Vector3 _currentMoveDirection;
    private float _changeDirectionTimer;

    // Start is called before the first frame update
    void Start() {
        _currentState = State.MoveRandomly;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMoveRandomly();
    }

    private void UpdateMoveToOrigin() {
        Vector3 directionToOrigin = _anchorPosition - transform.position;
        float distanceToOrigin = directionToOrigin.magnitude;
        directionToOrigin.Normalize();
        transform.Translate(directionToOrigin * (Time.deltaTime * moveSpeed));

        if (distanceToOrigin < 1) {
            _currentState = State.MoveRandomly;
        }
    }

    private void UpdateMoveRandomly() {
        // maybe try having enemies accelerate forwards/backwards but move sharply left/right
        // to look like theyre racing
        
        // pick a new direction
        if (_changeDirectionTimer <= 0) {
            Vector3 toAnchor = _anchorPosition - transform.position;
            // var distanceToAnchor = toAnchor.magnitude; 
            // todo: new direction has to be somewhat aligned to current direction, then decrease time
            
            Vector3 newDirection;
            bool newDirectionAccepted;
            do {
                newDirection = Random.onUnitSphere;
                var towardsAnchorAlignmentFactor = 0.6f;
                var currentDirectionAlignmentFactor = 0.4f;
                
                // Debug.Log("distance to anchor = " + toAnchor.magnitude + "/" + moveRadius);

                // if too far from anchor point, must be in direction of anchor point
                if (toAnchor != Vector3.zero && toAnchor.magnitude >= randomMoveRadius) {
                    if (toAnchor == Vector3.zero) newDirectionAccepted = true;
                    else {
                        var dot = Vector3.Dot(newDirection.normalized, toAnchor.normalized);
                        var f = (dot + 1) / 2.0f; // 1 when aligned, 0 when opposite 
                        newDirectionAccepted = (f >= towardsAnchorAlignmentFactor);
                    }
                }
                // otherwise, must be similar to current direction
                else {
                    if (_currentMoveDirection == Vector3.zero) newDirectionAccepted = true;
                    else {
                        var dot = Vector3.Dot(newDirection.normalized, _currentMoveDirection.normalized);
                        var f = (dot + 1) / 2.0f; // 1 when aligned, 0 when opposite 
                        newDirectionAccepted = (f >= currentDirectionAlignmentFactor);
                    }
                }
            } while (!newDirectionAccepted);
            
            // Debug.Log("newDirection = " + newDirection + " with f = " + f);

            _currentMoveDirection = newDirection;
            _changeDirectionTimer = changeDirectionTime;
        }
        // move in direction
        else {
            transform.Translate(_currentMoveDirection * (Time.deltaTime * moveSpeed));
            _changeDirectionTimer -= Time.deltaTime;
        }

    }

    public void SetAnchor(Vector3 position) {
        _anchorPosition = position;
    }
}
