using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlanet : MonoBehaviour
{
    public Transform Target;
    [SerializeField] private CinemachineVirtualCamera planetCamera;
    private CinemachineTransposer transposer;

    // PC Camera
    ///////////////////////////////////////////////////

    //Moving
    [SerializeField] private float minTurnAnglePC = -90.0f;
    [SerializeField] private float maxTurnAnglePC = 0.0f;

    [SerializeField] private float turnSpeedPC = 4.0f;
    private float MouseXPC;
    private float MouseYPC;

    private float rotXPC;
    //Moving

    //Zoom
    private float wheelSpeedPC;

    private float newZPC;

    [SerializeField] float zoomSpeedPC;

    [SerializeField] float minDistancePC;
    [SerializeField] float maxDistancePC;

    //private bool zoomLastPC = false;
    //Zoom

    // PC Camera
    ///////////////////////////////////////////////////


    // Android
    ///////////////////////////////////////////////////

    //Moving
    [SerializeField] private float minTurnAngleMobile = -90.0f;
    [SerializeField] private float maxTurnAngleMobile = 0.0f;

    [SerializeField] private float turnSpeedMobile = 4.0f;
    private float MouseXMobile;
    private float MouseYMobile;

    private float rotXMobile;
    //Moving

    //Zoom


    [SerializeField] float zoomSpeedMobile;

    [SerializeField] private float orthoZoomSpeedMobile;

    [SerializeField] private float minDistanceMobile;
    [SerializeField] private float maxDistanceMobile;

    private float wheelSpeedMobile;

    private bool zoomLastMobile = false;

    [SerializeField] private float targetDistanceMobile;

    private float newZMobile;
    //Zoom

    // Android
    ///////////////////////////////////////////////////


    private GameCycleManager gameCycleManager;

    void Start()
    {
        Input.simulateMouseWithTouches = true;
        gameCycleManager = FindObjectOfType<GameCycleManager>();
    }

    void Update()
    {
        if (Target != null)
        {
#if UNITY_EDITOR
            PCCameraUpdate();
            PCCameraZoom();
#endif
#if UNITY_ANDROID
            AndroidCameraUpdate();
            AndroidCameraZoom();
#endif
        }
    }

    private void PCCameraUpdate()
    {
        MouseXPC = Input.GetAxis("Mouse X");
        MouseYPC = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(0))
        {
            // get the mouse inputs
            float y = MouseXPC * turnSpeedPC;
            rotXPC += MouseYPC * turnSpeedPC;
            float rotZ = 0;
            // clamp the vertical rotation
            rotXPC = Mathf.Clamp(rotXPC, minTurnAnglePC, maxTurnAnglePC);

            // rotate the camera
            transform.eulerAngles = new Vector3(-rotXPC, transform.eulerAngles.y + y, rotZ);
        }

        transform.position = Target.position;
    }

    private void PCCameraZoom()
    {
        if (transposer == null)
        {
            transposer = planetCamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

        wheelSpeedPC = mouseWheelInput;

        if (wheelSpeedPC < 0)
        {
            newZPC = transposer.m_FollowOffset.z -= zoomSpeedPC;
        }

        if (wheelSpeedPC > 0)
        {
            newZPC = transposer.m_FollowOffset.z += zoomSpeedPC;
        }

        if (newZPC >= minDistancePC)
        {
            newZPC = minDistancePC;
        }
        else if (newZPC <= maxDistancePC)
        {
            newZPC = maxDistancePC;
        }

        transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, newZPC);
    }

    void AndroidCameraUpdate()
    {
        if (transposer == null)
        {
            transposer = planetCamera.GetCinemachineComponent<CinemachineTransposer>();
        }

        if (Input.touchCount == 1 && zoomLastMobile == false)
        {
            var touch = Input.GetTouch(0);

            MouseXMobile = Input.GetAxis("Mouse X");
            MouseYMobile = Input.GetAxis("Mouse Y");

            if (touch.phase == TouchPhase.Moved)
            {
                // get the mouse inputs
                float y = MouseXMobile * turnSpeedMobile;
                rotXMobile += MouseYMobile * turnSpeedMobile;

                // clamp the vertical rotation
                rotXMobile = Mathf.Clamp(rotXMobile, minTurnAngleMobile, maxTurnAngleMobile);

                // rotate the camera
                transform.eulerAngles = new Vector3(-rotXMobile, transform.eulerAngles.y + y, 0);
            }
        }

        transform.position = Target.position;

    }

    public void AndroidCameraZoom()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            zoomLastMobile = true;
            // Store both touches.
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Moved && touchOne.phase == TouchPhase.Moved)
            {
                // Find the position in the previous frame of each touch.
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                // Find the magnitude of the vector (the distance) between the touches in each frame.
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                // Find the difference in the distances between each frame.
                float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                targetDistanceMobile += deltaMagnitudeDiff * orthoZoomSpeedMobile;

                targetDistanceMobile = Mathf.Clamp(targetDistanceMobile, minDistanceMobile, maxDistanceMobile);

                float mouseWheelInput = deltaMagnitudeDiff;

                wheelSpeedMobile = mouseWheelInput;

                if (wheelSpeedMobile < 0)
                {
                    newZMobile = transposer.m_FollowOffset.z += zoomSpeedMobile;
                }

                if (wheelSpeedMobile > 0)
                {
                    newZMobile = transposer.m_FollowOffset.z -= zoomSpeedMobile;
                }

                if (newZMobile >= minDistanceMobile)
                {
                    newZMobile = minDistanceMobile;
                }
                else if (newZMobile <= maxDistanceMobile)
                {
                    newZMobile = maxDistanceMobile;
                }

                transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, newZMobile);

                gameCycleManager.uiManager.SetDebug(newZMobile.ToString());
            }
        }

        if (Input.touchCount == 0)
        {
            zoomLastMobile = false;
        }
    }

    public static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 360)
            return angle - 360;
        return angle;
    }


    public void SetTarget(Transform target)
    {
        Target = target;
    }
}
