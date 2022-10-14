using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
    public bool Started = false;

    public float orthoZoomSpeedPC = 5f;

    public float orthoZoomSpeedAndroid = 2f;

    public float zoomValue;

    public float zoomValueDebug;

    public float zoomLerpSpeed;

    [SerializeField] private float zoomMin = 3;
    [SerializeField] private float zoomMax = 50;

    public bool zoomLast = false;



    private void Start()
    {
        Application.targetFrameRate = 300;
        Input.simulateMouseWithTouches = true;
        Started = true;
    }

    private void Update()
    {
        if (Started)
        {
#if UNITY_EDITOR
            PCCameraZoom();
#endif
#if UNITY_ANDROID
            AndroidCameraZoom();
#endif
        }
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

                zoomValue += deltaMagnitudeDiff * orthoZoomSpeedAndroid;
                Vector3 myPosition = transform.position;
                float yPosition = myPosition.y + zoomValue;
                yPosition = Mathf.Clamp(yPosition, zoomMin, zoomMax);
                Vector3 newZoom = new Vector3(myPosition.x, yPosition, myPosition.z);
                transform.position = Vector3.Lerp(transform.position, newZoom, zoomLerpSpeed);

                zoomValue = 0;
            }
        }

        if (Input.touchCount == 0)
        {
            zoomLast = false;
        }
    }

    public void PCCameraZoom()
    {
        float mouseWheelInput = -Input.mouseScrollDelta.y;
        zoomValue += mouseWheelInput * orthoZoomSpeedPC;
        Vector3 myPosition = transform.position;
        float yPosition = myPosition.y + zoomValue;
        yPosition = Mathf.Clamp(yPosition, zoomMin, zoomMax);
        Vector3 newZoom = new Vector3(myPosition.x, yPosition, myPosition.z);
        transform.position = Vector3.Lerp(transform.position, newZoom, zoomLerpSpeed);
        zoomValue = 0;
    }
}