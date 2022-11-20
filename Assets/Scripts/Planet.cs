using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : SpaceObject
{
    public int AsteroidsCount;
    [SerializeField] private Transform center;

    [SerializeField] private float orbitRadiusCurrent = 42;
    [SerializeField] private float orbitRadiusStep = 6f;
    [SerializeField] private List<SpaceShark> unitsSpacePrefabs;
    [SerializeField] private Vector3 spawnLeftPosition;
    [SerializeField] private Vector3 spawnRightPosition;
    
    private void Start()
    {
        Init();
    }

    private void Init()
    {
        CreateAsteroids(AsteroidsCount);
    }

    private void CreateAsteroids(int asteroidCount)
    {
        var planetScript = FindObjectOfType<Planet>();
        center = planetScript.transform;
        int spaceObjectPrefabIndex = 0;

        for (int i = 0; i < asteroidCount; i++)
        {
            float randomY = Random.Range(-25, 25);
            Vector3 asteroidOffset = new Vector3(0, randomY, 0);
            Vector3 createPos = transform.position + asteroidOffset;

            var asteroid = Instantiate(spaceObjectsPrefabsAsteroids[spaceObjectPrefabIndex], createPos, Quaternion.identity);

            //asteroid.transform.parent = transform;

            Asteroid asteroidScript = asteroid.GetComponent<Asteroid>();

            asteroidScript.OrbitRadius = orbitRadiusCurrent;
            orbitRadiusCurrent += orbitRadiusStep;

            Vector3 asteroidPosition = transform.position;

            Orbit asteroidOrbit = asteroid.GetComponent<Orbit>();
            asteroidOrbit.SetTarget(transform);

            asteroids.Add(asteroidScript);
        }
    }

    private void SpawnChild(int unitIndex)
    {
        Vector3 createPosition = transform.position;

        int randomSide = Random.Range(0, 2);
        
        if (randomSide == 0)
        {
            createPosition = createPosition + spawnLeftPosition;
        }
        else
        {
            createPosition = createPosition + spawnRightPosition;
        }
        
        Instantiate(unitsSpacePrefabs[unitIndex], createPosition, Quaternion.identity);
    }

    public void ButtonCreateChild()
    {
        SpawnChild(0);
    }
}
