/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject target;

    public float cameraSensitivity = 1f;
    public float boomLength = 10f;

    private float scrollInput;
    private Vector2 cameraInput;

    private Vector3 cameraOffset;

    [SerializeField]
    private float camSmooth = 5f;
    [SerializeField]
    private float zoomSpeed = 2f;

    private PlayerController playerController;



    // Start is called before the first frame update
    void Start()
    {
        //set camera on player
        transform.position = target.transform.position;
    }
    private void Awake()
    {
        playerController = target.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        scrollInput = playerController.playerInput.Scroll;
        cameraInput = playerController.playerInput.Cam;

        Vector3 newPos = transform.position + cameraOffset;
        
    }

    void LateUpdate()
    {
        CameraControl();

    }

    void CameraControl()
    {
        boomLength -= scrollInput / 120 * zoomSpeed;
        boomLength = Mathf.Clamp(boomLength, 5, 40);


        

        Vector3 vecFromTarget = transform.position - target.transform.position;
        Vector3 newVec = Quaternion.AngleAxis(cameraInput.x * cameraSensitivity * Time.deltaTime, Vector3.up) * vecFromTarget;
        newVec = Quaternion.AngleAxis(cameraInput.y * cameraSensitivity * Time.deltaTime, Vector3.right) * newVec;
        newVec.Normalize();
        newVec.y = Mathf.Clamp(newVec.y, 0, 80);

        newVec *= boomLength;

        
        transform.LookAt(target.transform.position);
        transform.position = Vector3.Slerp(transform.position, target.transform.position + newVec, camSmooth);


        *//*boomLength -= scrollInput / 120 * zoomSpeed;
        boomLength = Mathf.Clamp(boomLength, 5, 40);


        Ray ray = new Ray(target.transform.position, transform.position - target.transform.position);
        if (Physics.Raycast(ray, out RaycastHit hit, boomLength))
        {
            if (hit.distance < boomLength)
            {
                cameraOffset = (target.transform.position - transform.position).normalized * hit.distance * 0.9f;
            }
        }
        else
        {
            cameraOffset = (target.transform.position - transform.position).normalized * boomLength;
        }

        transform.LookAt(target.transform.position);

        //set camera distance

        cameraOffset = Quaternion.AngleAxis(cameraInput.x * cameraSensitivity, Vector3.up) * cameraOffset;
        cameraOffset = Quaternion.AngleAxis(-cameraInput.y * cameraSensitivity, Vector3.right) * cameraOffset;*//*
    }
}
*/