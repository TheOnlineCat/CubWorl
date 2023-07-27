using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    internal GameObject target;

    public float sensitivity = 3;
    public float boomLength = 10f;

    public float minLength = 20f;
    public float maxLength = 70f;

    private CinemachineFreeLook cam;
    private PlayerStateMachine playerController;

    [SerializeField]
    private float zoomSpeed = 2f;
    void Start()
    {
        cam.m_CommonLens = true;
    }

    private void Awake()
    {
        cam = GetComponent<CinemachineFreeLook>();
        playerController = target.GetComponent<PlayerStateMachine>();
        

    }

    // Update is called once per frame
    void Update()
    {
        boomLength -= playerController.playerInput.scroll / 120 * zoomSpeed;
        boomLength = Mathf.Clamp(boomLength, minLength, maxLength);
        cam.m_Lens.FieldOfView = boomLength;

        cam.m_XAxis.m_MaxSpeed = sensitivity * 100;
        cam.m_YAxis.m_MaxSpeed = sensitivity;
    }
}
