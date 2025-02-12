using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour {
    // components 
    [SerializeField] private GameEvent movedToOrigin;
    
    // constants 
    [SerializeField] private float originMoveSpeed;
    [SerializeField] private MinMaxFloat randomMoveSpeed;
    [SerializeField] private Vector2 randomMoveArea;
    private Vector2 _screenBounds; // 8.5, 4.5
    
    // state 
    private enum MoveState { MoveToOrigin, MoveRandomly, MovePaused }
    private MoveState _currentMoveState;
    private Vector3 _anchorPosition;
    private Vector2 _perlinSamplePoint; 

    // Start is called before the first frame update
    void Start() {
        _screenBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));
        
        _currentMoveState = MoveState.MoveToOrigin;
        _perlinSamplePoint = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentMoveState == MoveState.MovePaused) return;
        if (_currentMoveState == MoveState.MoveToOrigin) UpdateMoveToOrigin();
        else UpdateMoveRandomly();
    }

    private void UpdateMoveToOrigin() {
        Vector3 directionToOrigin = _anchorPosition - transform.position;
        float distanceToOrigin = directionToOrigin.magnitude;
        directionToOrigin.Normalize();
        transform.Translate(directionToOrigin * (Time.deltaTime * originMoveSpeed));

        if (distanceToOrigin < 1) {
            _currentMoveState = MoveState.MoveRandomly;
            movedToOrigin.Raise();
        }
    }

    private void UpdateMoveRandomly() {
        if (IsOffscreen()) {
            _currentMoveState = MoveState.MoveToOrigin;
            _perlinSamplePoint = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            return;
        }
        
        PerlinMovement();
    }

    private void PerlinMovement() {
        float perlinSampleSpeed = 0.5f;
        float border = 0.2f;
        
        // get random perlin value for movement this frame 
        Vector2 perlin = Vector2.zero;
        perlin.x = Mathf.Clamp(Mathf.PerlinNoise1D(_perlinSamplePoint.x), 0, 1); // [0, 1]
        perlin.y = Mathf.Clamp(Mathf.PerlinNoise1D(_perlinSamplePoint.y), 0, 1); // [0, 1]
        _perlinSamplePoint += perlinSampleSpeed * Time.deltaTime * Vector2.one;

        // set midpoint based on bounds & constrain perlin output
        Vector3 offset = transform.position - _anchorPosition; // [-radius, +radius]
        Vector2 t;
        t.x = Mathf.InverseLerp(-randomMoveArea.x, randomMoveArea.x, offset.x);
        t.y = Mathf.InverseLerp(-randomMoveArea.y, randomMoveArea.y, offset.y);
        Vector2 midpoint = new Vector2(0.5f, 0.5f);
        if (t.x <= border) {
            var f = Mathf.InverseLerp(0f, border, t.x);
            midpoint.x = Mathf.Lerp(0f, 0.5f, f);
        }
        else if (t.x >= 1 - border) {
            var f = Mathf.InverseLerp(1f - border, 1f, t.x);
            midpoint.x = Mathf.Lerp(0.5f, 1f, f);
        }
        if (t.y <= border) {
            var f = Mathf.InverseLerp(0f, border, t.y);
            midpoint.y = Mathf.Lerp(0f, 0.5f, f);
        }
        else if (t.y >= 1 - border) {
            var f = Mathf.InverseLerp(1f - border, 1f, t.y);
            midpoint.y = Mathf.Lerp(0.5f, 1f, f);
        }

        // constrain perlin output based on midpoint
        Vector2 direction = Vector2.zero;
        Vector2 magnitude = Vector2.zero; // [0, 1]
        // X
        if (perlin.x < midpoint.x) {
            direction.x = -1;
            magnitude.x = Mathf.InverseLerp(0, midpoint.x, midpoint.x - perlin.x);
        }
        else if (perlin.x > midpoint.x) {
            direction.x = 1;
            magnitude.x = Mathf.InverseLerp(midpoint.x, 1, perlin.x);
        }
        // Y
        if (perlin.y < midpoint.y) {
            direction.y = -1;
            magnitude.y = Mathf.InverseLerp(0, midpoint.y, midpoint.y - perlin.y);
        }
        else if (perlin.y > midpoint.y) {
            direction.y = 1;
            magnitude.y = Mathf.InverseLerp(midpoint.y, 1, perlin.y);
        }

        // set speed based on magnitude
        Vector2 speed = Vector2.zero;
        if (magnitude.x != 0) {
            speed.x = randomMoveSpeed.LerpValue(magnitude.x);
        }
        if (magnitude.y != 0) {
            speed.y = randomMoveSpeed.LerpValue(magnitude.y);
        }

        // translate 
        Vector3 velocity = new Vector3(direction.x * speed.x, direction.y * speed.y, 0);
        transform.Translate(Time.deltaTime * velocity);
    }

    public void SetAnchor(Vector3 position) {
        _anchorPosition = position;
    }

    private bool IsOffscreen() {
        var pos = transform.position;
        bool offscreen = pos.x < -_screenBounds.x || pos.x > _screenBounds.x || pos.y < -_screenBounds.y ||
                         pos.y > _screenBounds.y;
        return offscreen;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_anchorPosition, new Vector3(2*randomMoveArea.x, 2*randomMoveArea.y, 1));
    }

    public void PauseMovement(float duration) => StartCoroutine(PauseMovementCoroutine(duration));

    private IEnumerator PauseMovementCoroutine(float duration)
    {
        MoveState previousState = _currentMoveState;
        _currentMoveState = MoveState.MovePaused;
        yield return new WaitForSeconds(duration);
        _currentMoveState = previousState;
    }
}
