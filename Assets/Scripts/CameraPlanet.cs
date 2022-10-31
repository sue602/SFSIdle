using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlanet : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public float MouseX;
    public float MouseY;

    public Transform Target;
    public float targetDistance;
    public float minDistance = 5;
    public float maxDistance = 400;
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 0.0f;
    private float rotX;

    public float Speed = 0.2f;

    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

    public Vector3 OffSetPos;
    public float lerpSpeed = 1f;

    private Vector3 startGamePos = new Vector3(0, 0, -30);
    private Quaternion startGameRotation = new Quaternion(0, 0, 0, 1);

    public bool zoomLast = false;

    private GameCycleManager gameCycleManager;

    [SerializeField] private CinemachineVirtualCamera planetCamera;
    [SerializeField] private CinemachineTransposer transposer;

    [SerializeField] private float wheelSpeed;

    [SerializeField] private float newZ = 0;

    [SerializeField] float zoomSpeed = 0.1f;

    [SerializeField] float followOffsetMin = -0.9f;
    [SerializeField] float followOffsetMax = -10f;

    void Start()
    {
        Input.simulateMouseWithTouches = true;
        transform.position = startGamePos;
        transform.rotation = Quaternion.Euler(startGameRotation.x, startGameRotation.y, startGameRotation.z);
        gameCycleManager = FindObjectOfType<GameCycleManager>();
        planetCamera = gameCycleManager.GetPlanetCamera();
        transposer = planetCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    void Update()
    {
        if (Target != null)
        {
#if UNITY_EDITOR
            PCCameraUpdate();
#endif
#if UNITY_ANDROID
            AndroidCameraUpdate();
            AndroidCameraZoom();
#endif
        }
    }

    void PCCameraUpdate()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        if (Input.GetMouseButton(0))
        {
            // get the mouse inputs
            float y = MouseX * turnSpeed;
            rotX += MouseY * turnSpeed;
            float rotZ = 0;
            // clamp the vertical rotation
            rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

            // rotate the camera
            transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, rotZ);
        }

        transform.position = Target.position;

        float mouseWheelInput = Input.GetAxis("Mouse ScrollWheel");

        wheelSpeed = mouseWheelInput;

        if (wheelSpeed < 0)
        {
            newZ = transposer.m_FollowOffset.z -= zoomSpeed;
        }

        if (wheelSpeed > 0)
        {
            newZ = transposer.m_FollowOffset.z += zoomSpeed;
        }

        if(newZ >= followOffsetMin)
        {
            newZ = followOffsetMin;
        }else if (newZ <= followOffsetMax)
        {
            newZ = followOffsetMax;
        }

        transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, newZ);

    }

    void AndroidCameraUpdate()
    {
        if (Input.touchCount == 1 && zoomLast == false)
        {
            var touch = Input.GetTouch(0);

            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");

            if (touch.phase == TouchPhase.Moved)
            {
                // get the mouse inputs
                float y = MouseX * turnSpeed;
                rotX += MouseY * turnSpeed;

                // clamp the vertical rotation
                rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

                // rotate the camera
                transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
            }
        }

        transform.position = Target.position;

    }

    public void AndroidCameraZoom()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
            zoomLast = true;
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

                targetDistance += deltaMagnitudeDiff * orthoZoomSpeed;

                targetDistance = Mathf.Clamp(targetDistance, minDistance, maxDistance);

                float mouseWheelInput = deltaMagnitudeDiff;

                wheelSpeed = mouseWheelInput;

                if (wheelSpeed < 0)
                {
                    newZ = transposer.m_FollowOffset.z += zoomSpeed;
                }

                if (wheelSpeed > 0)
                {
                    newZ = transposer.m_FollowOffset.z -= zoomSpeed;
                }

                if (newZ >= followOffsetMin)
                {
                    newZ = followOffsetMin;
                }
                else if (newZ <= followOffsetMax)
                {
                    newZ = followOffsetMax;
                }

                transposer.m_FollowOffset = new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, newZ);

                gameCycleManager.uiManager.SetDebug(mouseWheelInput.ToString());
            }
        }

        if (Input.touchCount == 0)
        {
            zoomLast = false;
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
