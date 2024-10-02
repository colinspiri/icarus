using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TurnCar : MonoBehaviour {
    [SerializeField] private float turnSpeed = 1;
    [SerializeField] private float maxAngle = 45f;
    
    public int TurnDirection { get; set; } = 0;
    
    private const float ZeroThreshold = 1f;

    private float _currentAngle = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnDirection == 0) {
            _currentAngle = Mathf.Lerp(_currentAngle, 0, turnSpeed * Time.deltaTime);
            SetRotation();
        }
        else {
            var targetAngle = -TurnDirection * maxAngle;
            _currentAngle = Mathf.Lerp(_currentAngle, targetAngle, turnSpeed * Time.deltaTime);
            SetRotation();
            //RotateInDirection(-TurnDirection);
        }
    }

    private void RotateInDirection(int direction) {
        if (direction < 0 && _currentAngle < -maxAngle) return;
        if(direction > 0 && _currentAngle > maxAngle) return;

        _currentAngle += direction * turnSpeed * Time.deltaTime;
    }

    private void SetRotation() {
        transform.rotation = Quaternion.Euler(0, 0, _currentAngle);
    }
}
