using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionInfo : MonoBehaviour
{
    private int currentId = -1;
    private string typeObject;
    public static event Action<string, TimeSpan> OnUpdateProductionUI;
    public static event Action OnClearProductionUI;

    public void ShowProductionInfo(int Id, string Name, TimeSpan remainingTime, string type)
    {
        currentId = Id;
        typeObject = type;
        UpdateProductionInfo(Name, remainingTime);
    }

    public void ClearProductionInfo()
    {
        currentId = -1;
        OnClearProductionUI?.Invoke();
    }

    private void UpdateProductionInfo(string Name, TimeSpan remainingTime)
    {
        OnUpdateProductionUI?.Invoke(Name, remainingTime);
    }

    private void Update()
    {
        if (currentId == -1) return;
        if (typeObject == "Animal")
        {
            Animal animal = AnimalManager.Instance.activeAnimals.Count > currentId
                ? AnimalManager.Instance.activeAnimals[currentId].GetComponent<Animal>()
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
        if (typeObject == "Tree")
        {
            AppleTree tree = TreeManager.Instance.activeTrees.Count > currentId
                ? TreeManager.Instance.activeTrees[currentId].GetComponent<AppleTree>()
                : null;

            if (tree != null && tree.State == TreeState.Growing)
            {
                UpdateProductionInfo("Apple Tree", tree.GetRemainingTime());
                Debug.Log(tree.GetRemainingTime());
            }
            else
            {
                ClearProductionInfo();
            }
        }
    }
}
