using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public Camera cam;
    public GameObject hand;
    public int gravityCoef = 3;
    public int jumpSpeed = 800;
    public int speed = 20;

    private float distToGround;

    [SerializeField]
    private InputActionReference movementInput, jumpInput, attackInput;

    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityCoef;

        distToGround = GetComponent<Collider>().bounds.extents.y;

    }

    // Update is called once per frame
    void Update()
    {
        Attack();
    }

    private void FixedUpdate()
    {
        MovementControl();
    }

    void MovementControl()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        Vector3 movement = new Vector3(movementInput.action.ReadValue<Vector3>().x, 0, movementInput.action.ReadValue<Vector3>().z);
        movement.Normalize();
        movement *= speed;

        movement *= Time.deltaTime;

        if (IsGrounded())
        {
            rb.AddForce(new Vector3(0, movementInput.action.ReadValue<Vector3>().y * jumpSpeed,0), ForceMode.Impulse);
        }

        rb.MoveRotation(cam.transform.rotation); 
        rb.MovePosition(rb.position + movement);


    }

    Boolean IsGrounded() 
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f);
    }


    void Attack()
    {
        Animator handAnimator = hand.GetComponent<Animator>();
        if (attackInput.action.WasPressedThisFrame())
        {
            handAnimator.SetTrigger("Attack");
        }
    }
}
