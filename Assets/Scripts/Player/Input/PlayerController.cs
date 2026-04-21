using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public event System.Action OnLevelEnd;
    public float speed = 4f;
    private Vector2 move;

    float angle;
    Rigidbody rb;
    bool disabled;

    private Animator animator;
    private CharacterController controller;

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GuardPath.OnGuardHasSpottedPlayer += Disable;

        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!disabled)
        {
            movePlayer();
        }
    }

    private void OnTriggerEnter(Collider hitCollider)
    {
      if (hitCollider.tag == "Finish")
        {
            Disable();
            if (OnLevelEnd != null)
            {
                OnLevelEnd();
            }
        }
    }

    private void Disable()
    {
        disabled = true;
    }

    public void movePlayer()
    {
        Vector3 movement = new Vector3(move.x, 0f, move.y);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15f);
        }

        bool confirmMovement = movement != Vector3.zero;

        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        animator.SetBool("isMoving", confirmMovement);
    }

    private void OnDestroy()
    {
        GuardPath.OnGuardHasSpottedPlayer -= Disable;
    }
}
