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
    private Dictionary<string, int> fedCounts = new Dictionary<string, int>();

    public List<GameObject> activeAnimals = new List<GameObject>(); 
    private ProductionInfo productionInfo;
    public InventorySystem inventorySystem;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        productionInfo = FindObjectOfType<ProductionInfo>();
        if (productionInfo == null)
        {
            Debug.LogError("ProductionInfo not found in the scene!");
        }
    }

    private void Start()
    {
        LoadAnimals();
    }

    private void LoadAnimals()
    {
        activeAnimals.Clear(); 
        foreach (var animalData in DataPlayer.allData.animalDataList)
        {
            SpawnAnimal(animalData);
        }
    } 
    public void SpawnAnimalAfterPlacement(Vector3 position, string animalTypeName)
    {
        int newId = DataPlayer.allData.animalDataList.Count;
        AnimalTypeData animalType = animalTypes.Find(x => x.animalName == animalTypeName);
        AnimalData animalData = new AnimalData(
            newId,
            "",
            animalType,
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

        activeAnimals.Add(animalObj); 
    }

    public void OnAnimalClicked(int animalId, bool isRightClick = false)
    {
        if (animalId < 0 || animalId >= activeAnimals.Count)
            return;

        Animal animal = activeAnimals[animalId].GetComponent<Animal>();

        if (isRightClick)
        {
            SellAnimal(animalId);
        }
        else
        {
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
    }

    private void TryFeedAnimal(Animal animal)
    {
        string foodName = animal.AnimalTypeData.foodName; 
        if (inventorySystem.HasItem(foodName, 1)) 
        {
            inventorySystem.SubItem(foodName, 1); 
            animal.Feed();
            DataPlayer.UpdateAnimalData(animal.Id, animal.State, animal.EndTimeString, animal.transform.position);

            string animalName = animal.AnimalTypeData.animalName;
            if (!fedCounts.ContainsKey(animalName))
                fedCounts[animalName] = 0;
            fedCounts[animalName]++;
            MissionSystem.Instance.TrackProgress(animalName, 1, MissionType.FeedAnimal);
            SoundManager.Ins.Proceed();
        }
        else
        {
            Debug.Log("You need " + foodName);
        }
    }
    public int GetFedCount(string animalName)
    {
        return fedCounts.ContainsKey(animalName) ? fedCounts[animalName] : 0;
    }
    private void ShowProductionProgress(Animal animal)
    {
        if (productionInfo != null)
        {
            productionInfo.ShowProductionInfo(animal.Id, animal.AnimalTypeData.animalName, animal.GetRemainingTime());
        }
    }

    private void CollectProduct(Animal animal)
    {
        string productName = animal.AnimalTypeData.productName; 
        inventorySystem.AddItem(productName, 1); 
        animal.ResetToHungry();
        DataPlayer.UpdateAnimalData(animal.Id, animal.State, animal.EndTimeString, animal.transform.position);
        MissionSystem.Instance.TrackProgress(productName, 1, MissionType.Collect);
        SoundManager.Ins.Proceed();
    }

    private void SellAnimal(int animalId)
    {
        if (animalId < 0 || animalId >= activeAnimals.Count)
            return;

        GameObject animalObj = activeAnimals[animalId];
        Destroy(animalObj);

        activeAnimals.RemoveAt(animalId);
        DataPlayer.RemoveAnimalData(animalId);

        for (int i = 0; i < activeAnimals.Count; i++)
        {
            Animal animal = activeAnimals[i].GetComponent<Animal>();
            animal.UpdateId(i);
            DataPlayer.allData.animalDataList[i].id = i;
        }


        if (productionInfo != null)
        {
            productionInfo.ClearProductionInfo();
        }
    }
}
