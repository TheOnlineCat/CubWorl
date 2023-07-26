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
            /*if (!Physics.Raycast(rb.position, movement, movement.magnitude + 0.2f))
            {
                rb.MovePosition(rb.position + movement);
            }*/
            /*rb.AddForce(movement);
            Vector3 newVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            newVel.Normalize();
            Vector3.ClampMagnitude(newVel, speed);
            rb.velocity = new Vector3(newVel.x, rb.velocity.y, newVel.z); */
            MovePosition(rb.position + movement);
        }
    }

    void JumpControl()
    {
        if (IsGrounded())
        {
            rb.AddForce(new Vector3(0, movementInput.y * jumpForce, 0), ForceMode.Impulse);
        }
    }

    void MovePosition(Vector3 position)
    {
        Vector3 oldVel = rb.velocity;
        //Get the position offset
        Vector3 delta = position - rb.position;
        //Get the speed required to reach it next frame
        Vector3 vel = delta / Time.fixedDeltaTime;

        //If you still want gravity, you can do this
        vel.y = oldVel.y;

        //If you want your rigidbody to not stop easily when hit
        //This is however untested, and you should probably use a damper system instead, like using Smoothdamp but only keeping the velocity component
        vel.x = Mathf.Abs(oldVel.x) > Mathf.Abs(vel.x) ? oldVel.x : vel.x;
        vel.z = Mathf.Abs(oldVel.z) > Mathf.Abs(vel.z) ? oldVel.z : vel.z;

        rb.velocity = vel;
}
}
