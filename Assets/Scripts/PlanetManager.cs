using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField] List<PlanetObject> planetObjectsPrefabs = new List<PlanetObject>();
    [SerializeField] List<PlanetCharacter> planetCharactersPrefabs = new List<PlanetCharacter>();
    [SerializeField] int baseFloorCount = 0;
    [SerializeField] int charactersCount = 0;
    private bool created = false;
    private BuildManager buildManager;

    private void Start()
    {
        Initing();
    }

    private void Initing()
    {
        buildManager = FindObjectOfType<BuildManager>();

        if (!created)
        {
            CreatePlanet();
        }
        else
        {
            LoadPlanet();
        }
    }

    private void CreatePlanet()
    {
        CreatePlanetBaseFloor(0,baseFloorCount); // base floor
        CreatePlanetCharacters(0, charactersCount); // characters
    }

    private void CreatePlanetBaseFloor(int index, int count)
    {
        for(int i = 0;i < count; i++)
        {
            Vector3 randomPoint = GetRandomVerticePosition();

            var planetObjectPrefab = planetObjectsPrefabs[index];

            var planetObject = Instantiate(planetObjectPrefab, randomPoint, Quaternion.identity);
            buildManager.AddBuildPlatform(planetObject);
        }
    }

    private void CreatePlanetCharacters(int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPoint = GetRandomVerticePosition();

            var planetCharacterPrefab = planetCharactersPrefabs[index];

            var planetCharacter = Instantiate(planetCharacterPrefab, randomPoint, Quaternion.identity);
            planetCharacter.transform.parent = null;
        }
    }

    private void LoadPlanet()
    {

    }

    private Vector3 GetVerticePosition(int vertice)
    {
        Vector3 verticeReturn = new Vector3(0, 0, 0);

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        var vertices = meshFilter.mesh.vertices;

        verticeReturn = transform.TransformPoint(vertices[vertice]);

        return verticeReturn;
    }

    public Vector3 GetRandomVerticePosition()
    {
        Vector3 verticeReturn = new Vector3(0, 0, 0);

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        var vertices = meshFilter.mesh.vertices;

        int randomIndex = Random.Range(0, vertices.Length);

        verticeReturn = transform.TransformPoint(vertices[randomIndex]);

        return verticeReturn;
    }

   private Vector3 GetVerticeRotation(Vector3 vertice)
    {
        Vector3 verticeReturn = new Vector3(0, 0, 0);

        Vector3 verticePosition = transform.TransformPoint(vertice);

        Vector3 planetPosition = transform.position;

        verticeReturn = planetPosition - verticePosition;

        return verticeReturn;
    }
}
