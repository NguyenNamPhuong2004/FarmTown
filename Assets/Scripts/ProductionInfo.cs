using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionInfo : MonoBehaviour
{
    private int currentAnimalId = -1;

    private void OnEnable()
    {
        AnimalManager.Instance.OnShowProductionInfo += HandleShowProductionInfo;
        AnimalManager.Instance.OnClearProductionInfo += HandleClearProductionInfo;
    }

    private void OnDisable()
    {
        AnimalManager.Instance.OnShowProductionInfo -= HandleShowProductionInfo;
        AnimalManager.Instance.OnClearProductionInfo -= HandleClearProductionInfo;
    }

    private void HandleShowProductionInfo(int animalId, string animalName, TimeSpan remainingTime)
    {
        currentAnimalId = animalId;
        UpdateProductionInfo(animalName, remainingTime);
    }

    private void HandleClearProductionInfo()
    {
        ClearProductionInfo();
    }

    private void UpdateProductionInfo(string animalName, TimeSpan remainingTime)
    {
        ProductionInfoUI ui = FindObjectOfType<ProductionInfoUI>();
        if (ui != null)
        {
            ui.UpdateDisplay(animalName, remainingTime);
        }
    }

    private void ClearProductionInfo()
    {
        currentAnimalId = -1;
        ProductionInfoUI ui = FindObjectOfType<ProductionInfoUI>();
        if (ui != null)
        {
            ui.ClearDisplay();
        }
    }

    private void Update()
    {
        if (currentAnimalId != -1)
        {
            Animal animal = AnimalManager.Instance.activeAnimals.ContainsKey(currentAnimalId)
                ? AnimalManager.Instance.activeAnimals[currentAnimalId].GetComponent<Animal>()
                : null;

            if (animal != null && animal.State == AnimalState.Full)
            {
                UpdateProductionInfo(animal.AnimalTypeData.animalName, animal.GetRemainingTime());
            }
            else
            {
                ClearProductionInfo();
            }
        }
    }
}
