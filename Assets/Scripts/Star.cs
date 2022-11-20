using System.Collections.Generic;
using UnityEngine;

public class Star : SpaceObject
{
    private Transform center;

    private void CreatePlanets(int planetCount)
    {
        int spaceObjectPrefabIndex = 0;

        float orbitRadiusCurrent = 15;

        float orbitRadiusStep = 10f;

        for (int i = 0; i < planetCount; i++)
        {
            float randomY = Random.Range(-25, 25);
            Vector3 asteroidOffset = new Vector3(0, randomY, 0);
            Vector3 createPos = transform.position + asteroidOffset;

            var planet = Instantiate(spaceObjectsPrefabsPlanets[spaceObjectPrefabIndex], createPos, Quaternion.identity);

            planet.transform.parent = center;

            Planet planetScript = planet.GetComponent<Planet>();

            //planetScript.OrbitRadius = orbitRadiusCurrent;
            orbitRadiusCurrent += orbitRadiusStep;

            Vector3 starPosition = transform.position;

            Orbit planetOrbit = planet.GetComponent<Orbit>();
            planetOrbit.SetTarget(transform);

            planets.Add(planetScript);
        }
    }
}
