using System.Collections.Generic;
using UnityEngine;

public class Star : SpaceObject
{
    public int PlanetCount;
    public List<Color> colors = new List<Color>();
    public Vector2 ScaleRange;
    public bool homeStar = false;

    private bool open = false;

    private void Start()
    {
        float scaleValue = Random.Range(ScaleRange.x, ScaleRange.y);
        transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);

        int randomCount = Random.Range(1, PlanetCount); // Random planet count 

        PlanetCount = randomCount; // Random planet count

        CreatePlanets(PlanetCount);
        SetRandomColor();
    }

    private void SetRandomColor()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        int randomColorIndex = Random.Range(0, colors.Count);
        meshRenderer.material.color = colors[randomColorIndex];

        Light myLight = GetComponentInChildren<Light>();
        myLight.color = meshRenderer.material.color;
    }

    private void CreatePlanets(int planetCount)
    {
        int spaceObjectPrefabIndex = 0;
        Vector3 createPos = transform.position;

        float orbitRadiusCurrent = 5;

        float orbitRadiusStep = 2.5f;

        for (int i = 0; i < planetCount; i++)
        {
            var planet = Instantiate(spaceObjectsPrefabs[spaceObjectPrefabIndex], createPos, Quaternion.identity);

            planet.transform.parent = transform;

            Planet planetScript = planet.GetComponent<Planet>();

            planetScript.OrbitRadius = orbitRadiusCurrent;
            orbitRadiusCurrent += orbitRadiusStep;

            Vector3 starPosition = transform.position;

            Orbit planetOrbit = planet.GetComponent<Orbit>();
            planetOrbit.SetTarget(transform);

            planets.Add(planetScript);
        }
    }

    public void Open()
    {
        if (!open)
        {
            open = true;
        }
    }
}
