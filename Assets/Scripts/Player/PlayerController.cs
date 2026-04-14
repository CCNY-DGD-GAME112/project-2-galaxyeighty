using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }


    // Update is called once per frame

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        rb.linearVelocity = new Vector3(moveInput.x, rb.linearVelocity.y, moveInput.y);
    }
    
}
