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
    private UIManager uiManager;

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
        uiManager.ButtonColonizeEnable(false);
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

        float cameraSpeed = 0.1f;

        Vector3 cameraPosition = cameraMain.transform.position;

        Vector3 homePosition = homeStar.transform.position;

        Vector3 startPosition = cameraPosition;

        Vector3 endPosition = new Vector3(homePosition.x, cameraPosition.y, homePosition.z);

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
}
