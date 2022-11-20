using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textDebug;
    [SerializeField] private TextMeshProUGUI textCrystal;
    [SerializeField] private TextMeshProUGUI textMoney;

    [SerializeField] private Button buttonUpdateStrength;
    [SerializeField] private Button buttonUpdateSpeed;
    [SerializeField] private Button buttonUpdateCount;

    public void ButtonsUpdateEnable(bool enable)
    {
        buttonUpdateStrength.gameObject.SetActive(enable);
        buttonUpdateSpeed.gameObject.SetActive(enable);
        buttonUpdateCount.gameObject.SetActive(enable);
    }

    public void SetDebug(string text)
    {
        textDebug.text = text;
    }

    public void SetCrystal(string text)
    {
        textCrystal.text = text;
    }

    public void SetMoney(string text)
    {
        textMoney.text = text;
    }
}
