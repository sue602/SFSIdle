using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Galaxy : MonoBehaviour
{
    [SerializeField] private List<SpaceObject> spaceObjects = new List<SpaceObject>();
    [SerializeField] private int size;
    [SerializeField] private float starOffSet;
    [SerializeField] private Vector2 positionRange;
    [SerializeField] float destroyDistance;
    [SerializeField] private List<Star> stars = new List<Star>();
    private float width;
    private float height;
    private float x;
    private float z;
    private int index = 0;
    private int prefabIndex = 0;

    private void Start()
    {
        CreateGalaxyTrue();
    }

    private void CreateGalaxy()
    {
        size = size / 2;

        for(int i = 0;i < size; i++)
        {
            for(int j = 0;j < size; j++)
            {
                x = Random.Range(-positionRange.x, positionRange.x);
                z = Random.Range(-positionRange.y, positionRange.y);
                Vector3 myPosition = transform.position;
                Vector3 createPosition = new Vector3(myPosition.x + starOffSet * i + x, myPosition.y, myPosition.z + starOffSet * j + z);
                var star = Instantiate(spaceObjects[prefabIndex], createPosition, Quaternion.identity);
                star.transform.parent = transform;
                width = myPosition.x + starOffSet * i;
                height = myPosition.z + starOffSet * j;
                Star starScript = star.GetComponent<Star>();
                stars.Add(starScript);
            }
        }
        transform.position = new Vector3(-width/2, 0, -height/2);
    }

    private void CreateGalaxyTrue()
    {
        size = size / 2;
        Vector3 startPos = transform.position;
        float smaler = 0.5f;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float galaxy_step = 10;
                float step = 1.1f; // Angle of Galaxy
                float indexSpace = step * (galaxy_step + index);
                float xx = (startPos.x + Mathf.Sin(indexSpace) * indexSpace) / smaler;
                float zz = (startPos.z + Mathf.Cos(indexSpace) * indexSpace) / smaler;


                Vector3 myPos = transform.position;
                Vector3 createPos = new Vector3(xx, 0, zz);
                var star = Instantiate(spaceObjects[prefabIndex],createPos,Quaternion.identity);
                Star starScript = star.GetComponent<Star>();
                stars.Add(starScript);
                index++;
            }
        }
    }

    public List<Star> GetStars()
    {
        return stars;
    }
}
