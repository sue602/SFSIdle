using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float turnSpeed = 4.0f;
    public float MouseX;
    public float MouseY;

    public Transform Target;
    public float targetDistance;
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 0.0f;
    private float rotX;

    public float Speed = 0.2f;

    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.

    public bool cameraMoved;
    public bool cameraZoom;
    public bool cameraMovedPressed;

    public Vector3 OffSetPos;
    public float lerpSpeed = 1f;

    private Vector3 startGamePos = new Vector3(0, 0, 0);
    private Quaternion startGameRotation = new Quaternion(0, 0, 0, 1);

    private Vector3 playGamePos = new Vector3(0, 0, -10);
    private Quaternion playGameRotation = new Quaternion(0, 0, 0, 1);

    public bool Started = false;

    void Start()
    {
        Input.simulateMouseWithTouches = true;
        transform.position = startGamePos;
        transform.rotation = Quaternion.Euler(startGameRotation.x, startGameRotation.y, startGameRotation.z);
    }

    void Update()
    {
        CameraUpdate();
    }

    void CameraUpdate()
    {
        if (Started && Target != null)
        {
#if UNITY_EDITOR
            PCCameraUpdate();
#endif
#if UNITY_ANDROID
            AndroidCameraUpdate();
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
        // move the camera position
        transform.position = Vector3.Lerp(transform.position, Target.position - (transform.forward * targetDistance), Speed);
    }

    void AndroidCameraUpdate()
    {
        if (Input.touchCount == 1)
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

        // move the camera position
        transform.position = Vector3.Lerp(transform.position, Target.position - (transform.forward * targetDistance), Speed);
    }

    public void Zoom()
    {
        // If there are two touches on the device...
        if (Input.touchCount == 2)
        {
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

                // If the camera is orthographic...
                if (Camera.main.orthographic)
                {
                    // ... change the orthographic size based on the change in distance between the touches.
                    Camera.main.orthographicSize += deltaMagnitudeDiff * orthoZoomSpeed;

                    // Make sure the orthographic size never drops below zero.
                    Camera.main.orthographicSize = Mathf.Max(Camera.main.orthographicSize, 0.1f);
                }
                else
                {
                    // Otherwise change the field of view based on the change in distance between the touches.
                    Camera.main.fieldOfView += deltaMagnitudeDiff * perspectiveZoomSpeed;

                    // Clamp the field of view to make sure it's between 0 and 180.
                    Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 10f, 90f);
                }
            }
        }
    }

    public static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 360)
            return angle - 360;
        return angle;
    }
}
