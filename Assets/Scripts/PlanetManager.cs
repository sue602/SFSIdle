using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [SerializeField] List<PlanetObject> planetObjectsPrefabs = new List<PlanetObject>();
    [SerializeField] List<PlanetCharacter> planetCharactersPrefabs = new List<PlanetCharacter>();
    [SerializeField] int baseFloorCount = 0;
    [SerializeField] int grassCount = 0;
    [SerializeField] int charactersCount = 0;
    private bool created = false;
    private BuildManager buildManager;
    [SerializeField] private List<Vector3> verticesUsed = new List<Vector3>(); // list of useed vertices

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
        CreatePlanetGrass(1,grassCount);
        CreatePlanetCharacters(0, charactersCount); // characters
    }

    private void CreatePlanetBaseFloor(int index, int count)
    {
        int arrayIndex = 0;

        for(int i = 0;i < count; i++)
        {
            Vector3 createPoint = GetVerticePosition(arrayIndex);

            verticesUsed.Add(createPoint);

            Vector3 rotation = GetVerticeRotation(createPoint);

            var planetObjectPrefab = planetObjectsPrefabs[index];

            var planetObject = Instantiate(planetObjectPrefab, createPoint, Quaternion.identity);

            planetObject.Index = arrayIndex;

            planetObject.transform.rotation = new Quaternion(planetObject.transform.rotation.x, Random.rotation.y, planetObject.transform.rotation.z, 1);

            //planetObject.transform.parent = transform;

            var normalVector = Vector3.Normalize(createPoint);  // Rotate to planet geometry

            planetObject.transform.up = normalVector; // Rotate to planet geometry

            buildManager.AddBuildPlatform(planetObject);
            arrayIndex += 5;
        }
    }

    private void CreatePlanetGrass(int index,int count)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();

        var vertices = meshFilter.mesh.vertices;
        int grassIndex = 1;

        for (int i = 0; i < count; i+=grassIndex)
        {
            Vector3 createPoint = GetVerticePosition(i);

            if (!VerticeUsed(createPoint))
            {
                Vector3 rotation = GetVerticeRotation(createPoint);

                var planetObjectPrefab = planetObjectsPrefabs[index];

                var planetObject = Instantiate(planetObjectPrefab, createPoint, Quaternion.identity);

                planetObject.Index = i;

                planetObject.transform.rotation = new Quaternion(planetObject.transform.rotation.x, Random.rotation.y, planetObject.transform.rotation.z, 1);

                var normalVector = Vector3.Normalize(createPoint);// Rotate to planet geometry

                planetObject.transform.up = normalVector;  // Rotate to planet geometry

                //planetObject.transform.parent = transform;
            }
        }
    }

    private void CreatePlanetCharacters(int index, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPoint = GetRandomVerticePosition();

            var planetCharacterPrefab = planetCharactersPrefabs[index];

            var planetCharacter = Instantiate(planetCharacterPrefab, randomPoint, Quaternion.identity);

            //planetCharacter.transform.parent = transform;
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

    private bool VerticeUsed(Vector3 vertice)
    {
        bool result = false;

        for(int i = 0;i < verticesUsed.Count; i++)
        {
            if(verticesUsed[i] == vertice)
            {
                result = true;
            }
        }

        return result;
    }
}
