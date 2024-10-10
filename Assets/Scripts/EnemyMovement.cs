using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    [SerializeField] private float moveSpeed;
    
    private enum State { MoveToOrigin, MoveRandomly }
    private State _currentState = State.MoveToOrigin;
    private Vector3 _originPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentState == State.MoveToOrigin) {
            UpdateMoveToOrigin();
        }
        else if (_currentState == State.MoveRandomly) {
            UpdateMoveRandomly();
        }
    }

    private void UpdateMoveToOrigin() {
        Vector3 directionToOrigin = _originPosition - transform.position;
        float distanceToOrigin = directionToOrigin.magnitude;
        directionToOrigin.Normalize();
        transform.Translate(directionToOrigin * (Time.deltaTime * moveSpeed));

        if (distanceToOrigin < 1) {
            _currentState = State.MoveRandomly;
        }
    }

    private void UpdateMoveRandomly() {
        // try having enemies accelerate forwards/backwards but move sharply left/right
        // to look like theyre racing
    }

    public void SetOrigin(Vector3 position) {
        _originPosition = position;
    }
}
