using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Vector3 MovingPos;
    public string RaycastHitTag;

    public float MouseX;
    public float MouseY;

    [SerializeField] private GraphicRaycaster m_Raycaster;
    [SerializeField] private PointerEventData m_PointerEventData;
    [SerializeField] private EventSystem m_EventSystem;

    private UIManager uiManager;
    private GameCycleManager gameCycleManager;
    private BuildManager buildManager;

    void Start()
    {
        Input.simulateMouseWithTouches = true;
        Application.targetFrameRate = 100;

        //Fetch the Raycaster from the GameObject (the Canvas)
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        //Fetch the Event System from the Scene
        m_EventSystem = FindObjectOfType<EventSystem>();

        uiManager = FindObjectOfType<UIManager>();
        gameCycleManager = FindObjectOfType<GameCycleManager>();
        buildManager = FindObjectOfType<BuildManager>();
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
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        var hit = SetPosition();

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (!UIRaycasted()) // if press mouse over UI
            {
                uiManager.BuildScrollViewEnable(false);
                buildManager.BuildPlatformAllArrowEnable(false);
            }

            if (hit.transform != null)
            {
                //rayast object here

                BuildObject buildObject = hit.transform.GetComponent<BuildObject>();

                if (buildObject != null)
                {
                    buildObject.Select();
                    uiManager.ButtonsUpdateEnable(false);
                    uiManager.BuildScrollViewEnable(true);
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
