using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    const float fpsMeasurePeriod = 0.5f;
    private int m_FpsAccumulator = 0;
    private float m_FpsNextPeriod = 0;
    private int m_CurrentFps;
    const string display = "{0} FPS";
    public TextMeshProUGUI m_Text;

    void Start()
    {
        m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
    }

    void Update()
    {
        GetFPS();
    }

    void GetFPS()
    {
        m_FpsAccumulator++;
        if (Time.realtimeSinceStartup > m_FpsNextPeriod)
        {
            m_CurrentFps = (int)(m_FpsAccumulator / fpsMeasurePeriod);
            m_FpsAccumulator = 0;
            m_FpsNextPeriod += fpsMeasurePeriod;
            m_Text.text = string.Format(display, m_CurrentFps);
        }
    }
}
