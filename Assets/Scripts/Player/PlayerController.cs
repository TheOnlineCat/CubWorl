using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public GameObject hand;
    public Rigidbody rb;
    public CharacterController character;

    public float gravityCoef = 3f;


    [SerializeField]
    internal PlayerInput playerInput;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Awake()
    {
        character = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        Physics.gravity *= gravityCoef;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
