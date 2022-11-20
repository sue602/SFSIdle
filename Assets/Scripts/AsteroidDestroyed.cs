using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDestroyed : MonoBehaviour
{
    private void Start()
    {
        var crystals = GetComponentsInChildren<Crystal>();

        for(int i = 0;i< crystals.Length; i++)
        {
            crystals[i].Activate(); // move to account ui
        }
    }
}
