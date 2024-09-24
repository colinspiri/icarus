using UnityEngine;

// This class controls player movement
// Script is based from Coursera Game Design and Development 1: 2D Shooter project from MSU
public class PlayerMovement : MonoBehaviour {
    public static PlayerMovement Instance;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private float moveSpeed;
    
    private void Awake() {
        Instance = this;
    }

    private void Start()
    {
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Get movement input from the inputManager
        Vector3 movementVector = new Vector3(InputManager.Instance.horizontalMoveAxis, InputManager.Instance.verticalMoveAxis, 0);
        // Move the player
        MovePlayer(movementVector);
    }


    private void MovePlayer(Vector3 movement) {
        // when moving forwards, move at moveSpeed
        // when moving backwards, move at backwardsMoveSpeed
        // if moving sideways, move at speed = halfway between them 

        float dot = Vector3.Dot(movement, transform.right);
        float f = (dot + 1) / 2.0f;
        // float speed = Mathf.Lerp(backwardsMoveSpeed, moveSpeed, f);
        // Debug.Log("dot = " + dot + ", f = " + f + ", speed = " + speed);
        
        // Move the player's transform
        transform.position = transform.position + (movement * Time.deltaTime * moveSpeed);
    }
}