using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyStation : BuildObject
{
    [SerializeField] private float energyValue;
    private GameCycleManager gameCycleManager;

    private void Start()
    {
        StartInit();
    }

    private void StartInit()
    {
        gameCycleManager = FindObjectOfType<GameCycleManager>();
        gameCycleManager.AddEnergy(energyValue);
    }

    private void OnDestroy()
    {
        gameCycleManager.RemoveEnergy(energyValue);
    }
}
