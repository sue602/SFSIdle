using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class ViewDrag : MonoBehaviour
{
    public float LerpSpeed;
    Vector3 hit_position = Vector3.zero;
    Vector3 current_position = Vector3.zero;
    Vector3 camera_position = Vector3.zero;

    // Use this for initialization

    [SerializeField] private GraphicRaycaster m_Raycaster;
    [SerializeField] private PointerEventData m_PointerEventData;
    [SerializeField] private EventSystem m_EventSystem;

    void Start()
    {
        Application.targetFrameRate = 300;
        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = FindObjectOfType<EventSystem>();
    }

    void Update()
    {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        if (Input.GetMouseButtonDown(0))
        {
            if (results.Count <= 0)
            {
                hit_position = Input.mousePosition;
                camera_position = transform.position;
            }
        }

        if (Input.GetMouseButton(0))
        {
            if (results.Count <= 0)
            {
                current_position = Input.mousePosition;
#if UNITY_EDITOR
                LeftMouseDragPC();
#endif
#if UNITY_ANDROID
                LeftMouseDragAndroid();
#endif
            }
        }
    }

    void LeftMouseDragPC()
    {
        // From the Unity3D docs: "The z position is in world units from the camera."  In my case I'm using the y-axis as height
        // with my camera facing back down the y-axis.  You can ignore this when the camera is orthograhic.
        current_position.z = hit_position.z = camera_position.y;

        // Get direction of movement.  (Note: Don't normalize, the magnitude of change is going to be Vector3.Distance(current_position-hit_position)
        // anyways.  
        Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

        // Invert direction to that terrain appears to move with the mouse.
        direction = direction * -1;

        Vector3 position = camera_position + direction;

        transform.position = Vector3.Lerp(transform.position, position, LerpSpeed);


    }

    void LeftMouseDragAndroid()
    {
        if (Input.touchCount == 1)
        {
            // From the Unity3D docs: "The z position is in world units from the camera."  In my case I'm using the y-axis as height
            // with my camera facing back down the y-axis.  You can ignore this when the camera is orthograhic.
            current_position.z = hit_position.z = camera_position.y;

            // Get direction of movement.  (Note: Don't normalize, the magnitude of change is going to be Vector3.Distance(current_position-hit_position)
            // anyways.  
            Vector3 direction = Camera.main.ScreenToWorldPoint(current_position) - Camera.main.ScreenToWorldPoint(hit_position);

            // Invert direction to that terrain appears to move with the mouse.
            direction = direction * -1;

            Vector3 position = camera_position + direction;

            transform.position = Vector3.Lerp(transform.position, position, LerpSpeed);
        }
    }
}