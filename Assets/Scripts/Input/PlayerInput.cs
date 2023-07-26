using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    public Vector3 movement;
    public Vector2 cam;
    public bool clicked = false;
    public float scroll;

    [SerializeField]
    private InputActionReference movementInput, cameraInput, attackInput, scrollInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = movementInput.action.ReadValue<Vector3>();
        cam = cameraInput.action.ReadValue<Vector2>();
        scroll = scrollInput.action.ReadValue<float>();
        if (!clicked) clicked = attackInput.action.WasPressedThisFrame();
    }
}
