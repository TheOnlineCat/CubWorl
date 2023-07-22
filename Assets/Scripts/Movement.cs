using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomArm : MonoBehaviour
{
    public Camera cam;
    public int speed = 50;
    public int speedStrafe = 50;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MovementControl();
    }
    
    void MovementControl()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        
        Vector3 movement = new Vector3(speed * inputX, 0, speedStrafe * inputY);

        movement *= Time.deltaTime;

        transform.rotation = cam.transform.rotation;
        transform.Translate(movement);
    }
}
