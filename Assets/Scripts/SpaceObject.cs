using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour
{
    [SerializeField] protected List<Planet> spaceObjectsPrefabs = new List<Planet>();
    public List<Planet> planets = new List<Planet>();
}
