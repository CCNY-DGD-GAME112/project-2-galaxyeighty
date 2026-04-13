using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 6;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        float inputMagnitude = inputDirection.magnitude;

        float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.up * targetAngle;

        transform.Translate(transform.forward * moveSpeed * Time.deltaTime, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
