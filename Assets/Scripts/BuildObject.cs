using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : PlanetObject
{
    private BuildManager buildManager;
    private bool select = false;
    private BuildObject buildCurrent = null;
    private int buildIndex = 0;

    private void Start()
    {
        buildManager = FindObjectOfType<BuildManager>();
    }

    private void Update()
    {
        if(buildCurrent != null)
        {
            buildCurrent.transform.rotation = transform.rotation;
        }
    }

    public void Select()
    {
        select = !select;
        if (buildCurrent == null)
        {
            Vector3 createPosition = transform.position;
            buildCurrent = Instantiate(buildManager.buildObjectsPrefabs[buildIndex], createPosition, Quaternion.identity);
        }
        else
        {
            Destroy(buildCurrent.gameObject);
            buildCurrent = null;
        }
    }
}
