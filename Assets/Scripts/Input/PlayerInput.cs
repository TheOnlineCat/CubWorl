using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    public Vector3 Movement;
    public float Jump;
    public Vector2 Cam;
    public bool Clicked = false;
    public float Scroll;

    [SerializeField]
    private InputActionReference movementInput, cameraInput, attackInput, scrollInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement = new Vector2(movementInput.action.ReadValue<Vector3>().x, movementInput.action.ReadValue<Vector3>().z);
        Jump = movementInput.action.ReadValue<Vector3>().y;
        Cam = cameraInput.action.ReadValue<Vector2>();
        Scroll = scrollInput.action.ReadValue<float>();
        if (!Clicked) Clicked = attackInput.action.WasPressedThisFrame();
    }
}
