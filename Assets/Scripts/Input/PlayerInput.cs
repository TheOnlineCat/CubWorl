using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    [NonSerialized] public Vector3 Movement;
    [NonSerialized] public bool Jump;
    [NonSerialized] public Vector2 Cam;
    [NonSerialized] public bool Clicked = false;
    [NonSerialized] public float Scroll;


    [NonSerialized] public bool[] Ability;


    [Header("Movement")]
    [SerializeField]
    private InputActionReference movementInput;
    [SerializeField]
    private InputActionReference jumpInput, cameraInput, attackInput, scrollInput;

    [Header("Abilities")]
    [SerializeField]
    private InputActionReference ability1;
    [SerializeField]
    private InputActionReference ability2, ability3;

    private void Start()
    {
        Ability = new bool[3];
    }

    void Update()
    {
        Movement = new Vector2(movementInput.action.ReadValue<Vector3>().x, movementInput.action.ReadValue<Vector3>().z);
        Cam = cameraInput.action.ReadValue<Vector2>();
        Scroll = scrollInput.action.ReadValue<float>();

        if (!Jump) Jump = jumpInput.action.WasPressedThisFrame();
        if (!Clicked) Clicked = attackInput.action.WasPressedThisFrame();

        Ability[0] = ability1.action.IsPressed();
        Ability[1] = ability2.action.IsPressed();
        Ability[2] = ability3.action.IsPressed();
    }
}
