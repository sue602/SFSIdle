using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Vector3 MovingPos;
    public string RaycastHitTag;
    private const string GraviterTag = "Graviter";
    private const string VehicleTag = "Vehicle";

    public float MouseX;
    public float MouseY;

    [SerializeField] private GraphicRaycaster m_Raycaster;
    [SerializeField] private PointerEventData m_PointerEventData;
    [SerializeField] private EventSystem m_EventSystem;

    void Start()
    {
        Input.simulateMouseWithTouches = true;
        Application.targetFrameRate = 100;

        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = FindObjectOfType<EventSystem>();
    }

    void Update()
    {
        Inputing();
    }

    public RaycastHit SetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            RaycastHitTag = hit.transform.tag;
            MovingPos = hit.point;
        }

        return hit;
    }

    private void Brain()
    {
        UIManager uiManager = FindObjectOfType<UIManager>();

        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        var hit = SetPosition();

        if (hit.transform != null)
        {
            //Star
            Star star = hit.transform.GetComponent<Star>();

            if (star != null)
            {
                
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (star != null && !star.homeStar)
                {
                    uiManager.ButtonColonizeEnable(true);
                }
                //Star
            }

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!UIRaycasted())
                {
                    uiManager.ButtonColonizeEnable(false);
                }

            }
        }
    }

    private void Inputing()
    {
        Brain();

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHitTag = "";
        }
    }

    private bool UIRaycasted()
    {
        bool raycated = false;

        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        if (results.Count > 0)
        {
            raycated = true;
        }

        return raycated;
    }
}
