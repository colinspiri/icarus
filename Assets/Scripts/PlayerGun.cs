using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput() {
        // point the gun towards the mouse
        Vector2 lookPosition = GetLookPosition();
        LookAtPoint(lookPosition);
    }
    
    public Vector2 GetLookPosition()
    {
        Vector2 result = transform.up;
        
        result = new Vector2(InputManager.Instance.horizontalLookAxis, InputManager.Instance.verticalLookAxis);
        
        return result;
    }
    
    private void LookAtPoint(Vector3 point)
    {
        if (Time.timeScale > 0)
        {
            // Rotate the player to look at the mouse.
            var screenToWorldPoint = Camera.main.ScreenToWorldPoint(point);
            Vector2 lookDirection = screenToWorldPoint - transform.position;

            transform.up = lookDirection;
        }
    }
}
