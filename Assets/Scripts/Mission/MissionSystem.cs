﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionSystem : MonoBehaviour
{
    public static MissionSystem Instance;

    [SerializeField] private List<Item> allItems; 
    [SerializeField] private InventorySystem inventorySystem; 

    private List<MissionData> currentMissions = new List<MissionData>();
    private DateTime nextResetTime; 

    public event Action OnMissionsUpdated; 
    public event Action OnMissionsCompleted; 

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        InitializeMissions();
    }

    private void InitializeMissions()
    {
        try
        {
            nextResetTime = DateTime.ParseExact(DataPlayer.GetNextResetTime(), "yyyy-MM-ddTHH:mm:ss", null);
            if (DateTime.Now >= nextResetTime)
            {
                Debug.Log(10);
                ResetMissions();
            }
            else
            {
                currentMissions = DataPlayer.GetMissions();
                OnMissionsUpdated?.Invoke();
                Debug.Log(11);
            }
        }
        catch
        {
            ResetMissions(); 
        }
    }

    private void ResetMissions()
    {
        currentMissions.Clear();

        for (int i = 0; i < 3; i++)
        {
            MissionData mission = GenerateRandomMission();
            currentMissions.Add(mission);
        }

        nextResetTime = DateTime.Now.AddHours(12);
        DataPlayer.SetMissions(currentMissions);
        DataPlayer.SetMissionProgress(new List<int> { 0, 0, 0 });
        DataPlayer.SetNextResetTime(nextResetTime.ToString("yyyy-MM-ddTHH:mm:ss"));
        DataPlayer.SetIsBigRewardClaimed(false);
        OnMissionsUpdated?.Invoke();
    }

    private MissionData GenerateRandomMission()
    {
        MissionData mission = new MissionData();

        
        int randomType = UnityEngine.Random.Range(0, 5); 
        Item randomItem = null;

        if (randomType == 0) 
        {
            List<Item> plants = new List<Item>();
            foreach (Item item in allItems)
            {
                if (item.itemType == ItemType.Plant)
                {
                    plants.Add(item);
                }
            }
            if (plants.Count == 0)
            {
                return GenerateRandomMission();
            }
            randomItem = plants[UnityEngine.Random.Range(0, plants.Count)];
            mission.missionType = randomItem.itemName == "Apple" ? MissionType.Harvest :
                (UnityEngine.Random.Range(0, 2) == 0 ? MissionType.Plant : MissionType.Harvest);
            mission.requiredAmount = UnityEngine.Random.Range(5, 16); 
        }
        else if (randomType == 1) 
        {
            List<Item> foods = new List<Item>();
            foreach (Item item in allItems)
            {
                if (item.itemType == ItemType.Food)
                {
                    foods.Add(item);
                }
            }
            if (foods.Count == 0)
            {
                return GenerateRandomMission();
            }
            randomItem = foods[UnityEngine.Random.Range(0, foods.Count)];
            mission.missionType = MissionType.Cook;
            mission.requiredAmount = UnityEngine.Random.Range(5, 11); 
        }
        else if (randomType == 2) 
        {
            List<Item> animals = new List<Item>();
            foreach (Item item in allItems)
            {
                if (item.itemType == ItemType.Animal)
                {
                    animals.Add(item);
                }
            }
            if (animals.Count == 0)
            {
                return GenerateRandomMission();
            }
            randomItem = animals[UnityEngine.Random.Range(0, animals.Count)];
            mission.missionType = MissionType.FeedAnimal;
            mission.requiredAmount = UnityEngine.Random.Range(5, 16); 
        }
        else if (randomType == 3) 
        {
            List<Item> products = new List<Item>();
            foreach (Item item in allItems)
            {
                if (item.itemType == ItemType.Product)
                {
                    products.Add(item);
                }
            }
            if (products.Count == 0)
            {
                return GenerateRandomMission();
            }
            randomItem = products[UnityEngine.Random.Range(0, products.Count)];
            mission.missionType = MissionType.Collect;
            mission.requiredAmount = UnityEngine.Random.Range(3, 6); 
        }
        else 
        {
            mission.missionType = MissionType.Deliver;
            mission.targetItem = "package";
            mission.requiredAmount = UnityEngine.Random.Range(3, 6); 
            mission.description = $"Deliver {mission.requiredAmount} packages";
            mission.itemSprite = null;
            return mission;
        }

        mission.targetItem = randomItem.itemName;
        mission.itemSprite = randomItem.itemSprite;
        mission.description = GetMissionDescription(mission.missionType, mission.targetItem, mission.requiredAmount);

        return mission;
    }

    private string GetMissionDescription(MissionType type, string itemName, int amount)
    {
        switch (type)
        {
            case MissionType.Plant:
                return $"Plant {amount} {itemName} plants";
            case MissionType.Harvest:
                return $"Harvest {amount} {itemName}";
            case MissionType.Cook:
                return $"Cook {amount} {itemName}";
            case MissionType.FeedAnimal:
                return $"Feed {amount} {itemName}";
            case MissionType.Collect:
                return $"Collect {amount} {itemName}";
            default:
                return "";
        }
    }

    public void TrackProgress(string itemName, int amount, MissionType type)
    {
        List<int> progress = DataPlayer.GetMissionProgress();

        for (int i = 0; i < currentMissions.Count; i++)
        {
            MissionData mission = currentMissions[i];
            if (mission.missionType == type && mission.targetItem == itemName && progress[i] != -1)
            {
                progress[i] += amount;
                DataPlayer.SetMissionProgress(progress);
                CheckMissionCompletion();
                OnMissionsUpdated?.Invoke();
            }
        }
    }

    public void CompleteMission(MissionData mission)
    {
        List<int> progress = DataPlayer.GetMissionProgress();
        int index = currentMissions.IndexOf(mission);

        if (index >= 0 && progress[index] >= mission.requiredAmount && progress[index] != -1)
        {
            DataPlayer.AddCoin(100);
            Debug.Log($"Mission completed: {mission.description}. Received 100 coins.");
            progress[index] = -1;
            DataPlayer.SetMissionProgress(progress);
            CheckAllMissionsCompleted();
        }
    }

    private void CheckMissionCompletion()
    {
        List<int> progress = DataPlayer.GetMissionProgress();

        for (int i = 0; i < currentMissions.Count; i++)
        {
            MissionData mission = currentMissions[i];
            if (progress[i] >= mission.requiredAmount && progress[i] != -1)
            {
                CompleteMission(mission);
            }
        }
    }

    private bool CheckAllMissionsCompleted()
    {
        List<int> progress = DataPlayer.GetMissionProgress();
        for (int i = 0; i < progress.Count; i++)
        {
            if (progress[i] != -1)
            {
                return false;
            }
        }
        OnMissionsCompleted?.Invoke();
        return true;
    }

    public void ClaimBigReward()
    {
        if (!CanClaimBigReward())
        {
            Debug.Log("Cannot claim big reward: Inventory is full, missions not completed, or reward already claimed.");
            return;
        }

        int coinReward = UnityEngine.Random.Range(100, 1001);
        DataPlayer.AddCoin(coinReward);
        SoundManager.Ins.CollectCoin();
        Debug.Log($"Claimed big reward: {coinReward} coins.");

        List<Item> validRewardItems = new List<Item>();
        foreach (Item item in allItems)
        {
            if (item.itemType != ItemType.Animal && item.itemType != ItemType.Building && item.itemType != ItemType.Tree)
            {
                validRewardItems.Add(item);
            }
        }

        for (int i = 0; i < 5; i++)
        {
            if (validRewardItems.Count > 0)
            {
                Item randomItem = validRewardItems[UnityEngine.Random.Range(0, validRewardItems.Count)];
                inventorySystem.AddItem(randomItem.itemName, 1);
            }
        }

        DataPlayer.SetIsBigRewardClaimed(true);
        ResetMissions();
    }

    private bool CanClaimBigReward()
    {
        return CheckAllMissionsCompleted() && !DataPlayer.IsInventoryMax() && !DataPlayer.GetIsBigRewardClaimed();
    }

    public List<MissionData> GetCurrentMissions()
    {
        return currentMissions;
    }

    public int GetMissionProgress(MissionData mission)
    {
        int index = currentMissions.IndexOf(mission);
        if (index >= 0)
        {
            List<int> progress = DataPlayer.GetMissionProgress();
            return progress[index];
        }
        return 0;
    }

    public TimeSpan GetTimeLeft()
    {
        return nextResetTime - DateTime.Now;
    }
}