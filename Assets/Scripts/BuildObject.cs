using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : PlanetObject
{
    private BuildManager buildManager;
    public BuildObject buildCurrent = null;
    private bool select = false;
    public Arrow arrow;

    private void Start()
    {
        buildManager = FindObjectOfType<BuildManager>();
        arrow = GetComponentInChildren<Arrow>();
        ArrowEnable(false);
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
        buildManager.BuildPlatformAllSelect(false); // unselect all platform
        select = !select;
        if (buildCurrent == null)
        {
            // Activate arrow selector
            buildManager.SetBuildPlatform(this);
        }
        else
        {
            Destroy(buildCurrent.gameObject);
            buildCurrent = null;
        }
    }

    public void ArrowEnable(bool enable)
    {
        if(arrow != null)
        {
            arrow.gameObject.SetActive(enable);
        }
    }

    public void SetSelection(bool selection)
    {
        select = selection;
    }
}
