using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionInfo : MonoBehaviour
{
    private int currentAnimalId = -1;
    public static event Action<string, TimeSpan> OnUpdateProductionUI;
    public static event Action OnClearProductionUI;

    public void ShowProductionInfo(int animalId, string animalName, TimeSpan remainingTime)
    {
        currentAnimalId = animalId;
        UpdateProductionInfo(animalName, remainingTime);
    }

    public void ClearProductionInfo()
    {
        currentAnimalId = -1;
        OnClearProductionUI?.Invoke();
    }

    private void UpdateProductionInfo(string animalName, TimeSpan remainingTime)
    {
        OnUpdateProductionUI?.Invoke(animalName, remainingTime);
    }

    private void Update()
    {
        if (currentAnimalId != -1)
        {
            Animal animal = AnimalManager.Instance.activeAnimals.Count > currentAnimalId
                ? AnimalManager.Instance.activeAnimals[currentAnimalId].GetComponent<Animal>()
                : null;

            if (animal != null && animal.State == AnimalState.Full)
            {
                UpdateProductionInfo(animal.AnimalTypeData.animalName, animal.GetRemainingTime());
                Debug.Log(animal.GetRemainingTime());
            }
            else
            {
                ClearProductionInfo();
            }
        }
    }
}
