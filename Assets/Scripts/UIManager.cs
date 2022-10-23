using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button buttonColozine;
    [SerializeField] private Image iconHome;
    [SerializeField] private Button buttonShowHome;
    [SerializeField] private Button buttonLand;

    public void ButtonLandEnable(bool enable)
    {
        buttonLand.gameObject.SetActive(enable);
    }

    public void ButtonColonizeEnable(bool enable)
    {
        buttonColozine.gameObject.SetActive(enable);
    }

    public void IconHomeSetPosition(Vector3 position)
    {
        iconHome.transform.position = position;
    }

    public void ButtonShowHomeEnable(bool enable)
    {
        buttonShowHome.gameObject.SetActive(enable);
    }

    public void ShowHomeButtonStatusUpdate(Star starRenderer)
    {
        Renderer renderer = starRenderer.GetComponent<Renderer>();

        if (renderer.isVisible)
        {
            ButtonShowHomeEnable(false);
        }
        else
        {
            ButtonShowHomeEnable(true);
        }
    }
}
