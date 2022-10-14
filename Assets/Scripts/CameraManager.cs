using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Vector3 direction;
    private Vector3 startPosition;
    private Vector3 movingPosition;
    [SerializeField] private float speed;
    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;
    [SerializeField] private float speedLerp;
    [SerializeField] private float mouseX;
    [SerializeField] private float mouseY;
    [SerializeField] private float mouseSpeedMin;
    [SerializeField] private bool mouseMoving;
    [SerializeField] private bool mousePressed = false;

    private void Start()
    {
        Input.simulateMouseWithTouches = true;
    }

    private void Update()
    {
#if UNITY_EDITOR
        PCScroll();
#endif
#if UNITY_ANDROID
        MobileScroll();
#endif
    }

    private void PCScroll()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startPosition = Input.mousePosition;
            mousePressed = true;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            movingPosition = Input.mousePosition;

            direction = movingPosition - startPosition;
            direction = new Vector3(direction.x + Input.GetAxis("Mouse X"), direction.y, direction.z + Input.GetAxis("Mouse Y"));
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {

                speed = Mathf.Lerp(speed, speedMax, speedLerp);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            mousePressed = false;
        }

        if(mousePressed == false)
        {
            speed = Mathf.Lerp(speed, speedMin, speedLerp);
        }

        Vector3 newPosition = transform.position + new Vector3(-direction.x * Time.deltaTime, 0, -direction.y * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, newPosition, speed);

        speed = Mathf.Clamp(speed, speedMin, speedMax);
    }

    private void MobileScroll()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (Input.touchCount == 1)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    startPosition = Input.mousePosition;
                    mousePressed = true;
                }

                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    movingPosition = Input.mousePosition;

                    direction = movingPosition - startPosition;

                    direction = new Vector3(direction.x + Input.GetAxis("Mouse X"), direction.y, direction.z + Input.GetAxis("Mouse Y"));

                    if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                    {
                        speed = Mathf.Lerp(speed, speedMax, speedLerp);
                    }
                }

                if(touch.phase == TouchPhase.Moved)
                {
                    speed = Mathf.Lerp(speed, speedMax, speedLerp);
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            mousePressed = false;
        }

        if (mousePressed == false)
        {
            speed = Mathf.Lerp(speed, speedMin, speedLerp);
        }

        Vector3 newPosition = transform.position + new Vector3(-direction.x * Time.deltaTime, 0, -direction.y * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, newPosition, speed);

        speed = Mathf.Clamp(speed, speedMin, speedMax);
    }
}
