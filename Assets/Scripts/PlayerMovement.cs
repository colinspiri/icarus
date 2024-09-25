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
        transform.position = transform.position + (movement * Time.deltaTime * moveSpeed);
    }
}