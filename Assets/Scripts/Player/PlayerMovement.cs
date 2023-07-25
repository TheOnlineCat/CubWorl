using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 800.0f;
    public float speed = 20.0f;
    public float turnSmoothTime = 0.1f;

    private Rigidbody rb;
    private Camera cam;

    private Vector3 movementInput;
    private float distToGround;
    private float curSmoothVelocity;

    [SerializeField]
    private PlayerController playerController;



    // Start is called before the first frame update
    void Start()
    {
        rb = playerController.rb;
        cam = playerController.cam;
        distToGround = playerController.GetComponent<Collider>().bounds.extents.y;
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        movementInput = playerController.playerInput.movement;
    }

    private void FixedUpdate()
    {
        JumpControl();
        MovementControl();
        
    }

    Boolean IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    void MovementControl()
    {
        Vector3 direction = new Vector3(movementInput.x, 0, movementInput.z);

        if (direction.magnitude != 0)
        {
            float angleDirection = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            angleDirection += cam.transform.eulerAngles.y;

            float angleSmooth = Mathf.SmoothDampAngle(transform.eulerAngles.y, angleDirection, ref curSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angleSmooth, 0f);

            Vector3 movement = Quaternion.Euler(0f, angleDirection, 0f) * Vector3.forward;
            movement *= speed;
            movement *= Time.deltaTime;
            print(movement);
            rb.MovePosition(rb.position + movement);
        }
    }

    void JumpControl()
    {
        if (IsGrounded())
        {
            rb.AddForce(new Vector3(0, movementInput.y * jumpForce, 0), ForceMode.Impulse);
        }
    }
}
