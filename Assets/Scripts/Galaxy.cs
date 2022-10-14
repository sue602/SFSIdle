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

    private void Start()
    {
        CreateGalaxy();
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
                int prefabIndex = 0;
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

    public List<Star> GetStars()
    {
        return stars;
    }
}
