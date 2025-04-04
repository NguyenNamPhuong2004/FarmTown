using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;
    public List<AnimalTypeData> animalTypes;
    public GameObject animalPrefab;
    public Transform animalParent;

    public Dictionary<int, GameObject> activeAnimals = new Dictionary<int, GameObject>();
    private int nextAnimalId = 0;

    public event Action<int, string, TimeSpan> OnShowProductionInfo;
    public event Action OnClearProductionInfo;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        LoadAnimals();
    }

    private void LoadAnimals()
    {
        foreach (var animalData in DataPlayer.allData.animalDataList)
        {
            SpawnAnimal(animalData);
        }

        foreach (var animalData in DataPlayer.allData.animalDataList)
        {
            if (animalData.id >= nextAnimalId)
                nextAnimalId = animalData.id + 1;
        }
    }

    public void BuyAnimal(int animalTypeIndex, Vector3 position)
    {
        if (animalTypeIndex < 0 || animalTypeIndex >= animalTypes.Count)
        {
            Debug.LogError("Invalid animal type index: " + animalTypeIndex);
            return;
        }

        AnimalData animalData = new AnimalData(
            nextAnimalId++,
            "",
            animalTypes[animalTypeIndex],
            AnimalState.Hungry,
            position
        );

        DataPlayer.AddAnimalData(animalData);

        SpawnAnimal(animalData);
    }

    private void SpawnAnimal(AnimalData animalData)
    {
        GameObject animalObj = Instantiate(animalPrefab, animalData.position, Quaternion.identity, animalParent);
        Animal animalComponent = animalObj.GetComponent<Animal>();

        animalComponent.Initialize(animalData);

        activeAnimals[animalData.id] = animalObj;
    }

    public void OnAnimalClicked(int animalId)
    {
        if (!activeAnimals.ContainsKey(animalId))
            return;

        Animal animal = activeAnimals[animalId].GetComponent<Animal>();

        switch (animal.State)
        {
            case AnimalState.Hungry:
                TryFeedAnimal(animal);
                break;

            case AnimalState.Full:
                ShowProductionProgress(animal);
                break;

            case AnimalState.Product:
                CollectProduct(animal);
                break;
        }
    }

    private void TryFeedAnimal(Animal animal)
    {
        int foodItemId = GetFoodItemIdForAnimal(animal.AnimalTypeData.animalName);

        if (DataPlayer.GetItemQuantity(foodItemId) > 0)
        {
            DataPlayer.SubItemQuantity(foodItemId, 1);
            animal.Feed();
            UpdateAnimalData(animal);
        }
        else
        {
            Debug.Log("You need " + GetFoodNameForAnimal(animal.AnimalTypeData.animalName));
        }
    }

    private int GetFoodItemIdForAnimal(string animalName)
    {
        switch (animalName.ToLower())
        {
            case "cow": return 1;
            case "chicken": return 2;
            case "pig": return 3;
            default: return 0;
        }
    }

    private string GetFoodNameForAnimal(string animalName)
    {
        switch (animalName.ToLower())
        {
            case "cow": return "Cattle Feed";
            case "chicken": return "Chicken Feed";
            case "pig": return "Pig Feed";
            default: return "Animal Feed";
        }
    }

    private void ShowProductionProgress(Animal animal)
    { 
        OnShowProductionInfo?.Invoke(animal.Id, animal.AnimalTypeData.animalName, animal.GetRemainingTime());
    }

    private void CollectProduct(Animal animal)
    {
        int productId = GetProductIdForAnimal(animal.AnimalTypeData.animalName);
        DataPlayer.AddItemQuantity(productId, 1);
        animal.ResetToHungry();
        UpdateAnimalData(animal);
        OnClearProductionInfo?.Invoke();
    }

    private int GetProductIdForAnimal(string animalName)
    {
        switch (animalName.ToLower())
        {
            case "cow": return 4;
            case "chicken": return 5;
            case "pig": return 6;
            default: return 0;
        }
    }

    private void UpdateAnimalData(Animal animal)
    {
        for (int i = 0; i < DataPlayer.allData.animalDataList.Count; i++)
        {
            if (DataPlayer.allData.animalDataList[i].id == animal.Id)
            {
                AnimalData animalData = DataPlayer.allData.animalDataList[i];
                animalData.state = animal.State;
                animalData.endTime = animal.EndTimeString;
                animalData.position = animal.transform.position;
                break;
            }
        }
    }
    public void ClearProductionInfo()
    {
        OnClearProductionInfo?.Invoke();
    }
}
