using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public List<BuildObject> buildObjectsPrefabs = new List<BuildObject>();
    private List<BuildObject> buildPlatforms = new List<BuildObject>();

    private BuildObject currentBuildPlatform;
    private UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    public void Build(int buildIndex)
    {
        Vector3 createPosition = currentBuildPlatform.transform.position;
        currentBuildPlatform.buildCurrent = Instantiate(buildObjectsPrefabs[buildIndex], createPosition, Quaternion.identity);
        uiManager.BuildScrollViewEnable(false);
        currentBuildPlatform.ArrowEnable(false);
    }

    public void SetBuildPlatform(BuildObject buildObject)
    {
        currentBuildPlatform = buildObject;
        BuildPlatformAllArrowEnable(false);
        currentBuildPlatform.ArrowEnable(true);
    }

    public void AddBuildPlatform(PlanetObject planetObject) 
    {
        BuildObject buildObject = planetObject.GetComponent<BuildObject>();
        buildPlatforms.Add(buildObject);
    }

    public void BuildPlatformAllArrowEnable(bool enable)
    {
        for(int i =0;i< buildPlatforms.Count; i++)
        {
            buildPlatforms[i].ArrowEnable(enable);
        }
    }

    public void BuildPlatformAllSelect(bool select)
    {
        for (int i = 0; i < buildPlatforms.Count; i++)
        {
            buildPlatforms[i].SetSelection(select);
        }
    }
}
