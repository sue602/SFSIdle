using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCycleManager : MonoBehaviour
{
    [SerializeField] private bool homePlanetSelected = false;
    [SerializeField] private bool homePlanetMarked = false;
    [SerializeField] private bool homeShow = false;
    private Galaxy galaxy;
    private Star homeStar;
    private CharacterManager characterManager;
    private Planet currentPlanet = null;
    private CameraManager cameraManager;
    [SerializeField] private CinemachineVirtualCamera cinemachineCameraSpace;
    [SerializeField] private CinemachineVirtualCamera cinemachineCameraPlanet;

    public UIManager uiManager;

    private void Start()
    {
        StartInit();
    }

    private void LateUpdate()
    {
        SetHome();
    }


    private void StartInit()
    {
        uiManager = FindObjectOfType<UIManager>();
        characterManager = FindObjectOfType<CharacterManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        uiManager.ButtonColonizeEnable(false);
        uiManager.ButtonLandEnable(false);
    }

    private void SetHome() // home star selection
    {
        if(galaxy == null)
        {
            galaxy = FindObjectOfType<Galaxy>();
        }

        if (!homePlanetSelected)
        {
            List<Star> stars = galaxy.GetStars();
            int randomStarIndex = Random.Range(0, stars.Count);
            homeStar = stars[randomStarIndex];
            Camera mainCamera = Camera.main;
            Vector3 cameraPosition = mainCamera.transform.position;
            mainCamera.transform.position = new Vector3(homeStar.transform.position.x, cameraPosition.y, homeStar.transform.position.z);
            homeStar.homeStar = true; // Set home
            homePlanetSelected = true;
        }
        else
        {
            if (!homePlanetMarked)
            {
                Renderer homeRenderer = homeStar.GetComponent<Renderer>();
                homeRenderer.material.color = Color.white;
                homePlanetMarked = true;
            }
            else
            {
                Vector3 starPosition = Camera.main.WorldToScreenPoint(homeStar.transform.position);
                uiManager.IconHomeSetPosition(starPosition);
                uiManager.ShowHomeButtonStatusUpdate(homeStar);
                ShowHome();
            }
        }
    }

    private void ShowHome()
    {
        Camera cameraMain = Camera.main;

        float cameraSpeed = 0.05f;

        Vector3 cameraPosition = cameraMain.transform.position;

        Vector3 homePosition = homeStar.transform.position;

        Vector3 startPosition = cameraPosition;

        Vector3 endPosition = new Vector3(homePosition.x, cameraPosition.y, homePosition.z - (cameraPosition.z + 144)); // Bad code, must rewrite in future

        float stopDistance = 0.1f;

        if(Vector3.Distance(cameraPosition,endPosition) <= stopDistance)
        {
            homeShow = false;
        }

        if (homeShow)
        {
            cameraMain.transform.position = Vector3.Lerp(cameraPosition,endPosition, cameraSpeed);
        }
    }

    public void ButtonShowHome()
    {
        homeShow = true;
    }

    public void ButtonCreateCharacterAtPlanet()
    {
        //Vector3 createPosition = new Vector3(currentPlanet.transform.position.x, currentPlanet.transform.position.y + 3, currentPlanet.transform.position.z);
        //characterManager.CreateCharacter(0,createPosition);
        cinemachineCameraSpace.Priority = 1; // Set Space camera to low priority
        cinemachineCameraPlanet.Priority = 2; // Set main camera - planet camera
        //cinemachineCameraPlanet.Follow = currentPlanet.transform;
        //cinemachineCameraPlanet.LookAt = currentPlanet.transform;
        CameraPlanet cameraPlanet = FindObjectOfType<CameraPlanet>();
        cameraPlanet.Target = currentPlanet.transform;
        uiManager.ButtonLandEnable(false);
    }

    public void SetCurrentPlanet(Planet planet)
    {
        currentPlanet = planet;
    }

    public CinemachineVirtualCamera GetPlanetCamera()
    {
        return cinemachineCameraPlanet;
    }
}
