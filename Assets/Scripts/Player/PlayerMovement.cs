using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float jumpForce = 800.0f;
    public float speed = 20.0f;

    private Rigidbody rb;
    private Camera cam;
    private Vector3 movementInput;

    private float distToGround;
    private Vector3 movement;

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
        print(movementInput);
        movementInput = playerController.playerInput.movement;
    }

    private void FixedUpdate()
    {

        print(movement);

        MovementControl();
    }

    Boolean IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }

    void MovementControl()
    {
        movement = new Vector3(movementInput.x, 0, movementInput.z);
        movement.Normalize();
        movement *= speed;

        movement *= Time.deltaTime;

        //movement = Quaternion.Euler(0, cam.transform.rotation.y, 0) * movement;

        if (IsGrounded())
        {
            rb.AddForce(new Vector3(0, movementInput.y * jumpForce, 0), ForceMode.Impulse);
        }

/*        if (movementInput.magnitude != 0)
        {
            transform.Rotate(new Vector3(0, cam.transform.rotation.y, 0));
        }*/

        rb.MovePosition(rb.position + movement);
    }
}
