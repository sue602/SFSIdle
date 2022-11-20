using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    [SerializeField] protected List<Planet> spaceObjectsPrefabsPlanets = new List<Planet>();
    [SerializeField] protected List<Asteroid> spaceObjectsPrefabsAsteroids = new List<Asteroid>();
    protected List<Planet> planets = new List<Planet>();
    protected List<Asteroid> asteroids = new List<Asteroid>();
}
