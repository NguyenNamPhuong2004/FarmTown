using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataPlayer 
{
    private const string ALL_DATA = "all_data";
    public static AllData allData;

    public static event Action UpdateCoinEvent;
    static DataPlayer()
    {
        allData = JsonUtility.FromJson<AllData>(PlayerPrefs.GetString(ALL_DATA));
        if (allData == null)
        {
            allData = new AllData
            {
                tileDataList = new List<TileData>(),
                animalDataList = new List<AnimalData>(),
                itemQuantityList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                waitingSlotList = new List<WaitingSlot>(),
                coin = 1000,
                inventoryMax = 50,
                totalItemQuantity = 0,
                music = 0.5f,
                sound = 0.5f,
                skipCountToday = 0,
                lastResetDate = DateTime.Now.ToString("yyyy-MM-dd"),
                currentOrder = null,
                missions = new List<MissionData>(),
                missionProgress = new List<int> { 0, 0, 0 },
                nextResetTime = DateTime.Now.AddHours(12).ToString("yyyy-MM-ddTHH:mm:ss"),
                isBigRewardClaimed = false
        };
            SaveData();
        }
    }
    private static void SaveData()
    {
        var data = JsonUtility.ToJson(allData);
        PlayerPrefs.SetString(ALL_DATA, data);
    }
    public static void AddTileData(TileData tileData)
    {
        allData.AddTileData(tileData);
        SaveData();
    }
    public static void RemoveTileData(TileData tileData)
    {
        allData.RemoveTileData(tileData);
        SaveData();
    } 
    public static void AddWaitingSlot(WaitingSlot waitingSlot)
    {
        allData.AddWaitingSlot(waitingSlot);
        SaveData();
    }
    public static void RemoveWaitingSlot(WaitingSlot waitingSlot)
    {
        allData.RemoveWaitingSlot(waitingSlot);
        SaveData();
    }
    public static WaitingSlot GetWaitingSlot(int id)
    {
        return allData.GetWaitingSlot(id);
    }
    public static int GetWaitingSlotListCount()
    {
        return allData.GetWaitingSlotListCount();
    }
    public static List<TileData> GetTileDataList()
    {
        return allData.GetTileDataList();
    } 
    public static bool IsContain(int x, int y)
    {
        return allData.IsContain(x, y);
    }
    public static TileData GetTileDataFromList(int x, int y)
    {
        return allData.GetTileDataFromList(x, y);
    }
    public static void AddItemQuantity(int id, int quantity)
    {
        allData.AddItemQuantity(id, quantity);
        SaveData();
    }
    public static void SubItemQuantity(int id, int quantity)
    {
        allData.SubItemQuantity(id, quantity);
        SaveData();
    }
    public static int GetItemQuantity(int id)
    {
        return allData.GetItemQuantity(id);
    }
    public static int GetCoin()
    {
        return allData.GetCoin();
    }
    public static void SubCoin(int cost)
    {
        allData.SubCoin(cost);
        UpdateCoinEvent?.Invoke();
        SaveData();
    }
    public static bool IsEnoughMoney(int cost)
    {
        return allData.IsEnoughMoney(cost);
    }
    public static void AddCoin(int money)
    {
        allData.AddCoin(money);
        UpdateCoinEvent?.Invoke();
        SaveData();
    }
    public static void SetInventoryMax()
    {
        allData.SetInventoryMax();
        SaveData();
    }
    public static int GetInventoryMax()
    {
        return allData.GetInventoryMax();
    }
    public static int GetTotalItemQuantity()
    {
        return allData.GetTotalItemQuantity();
    } 
    public static bool IsInventoryMax()
    {
        return allData.IsInventoryMax();
    }
    public static void AddAnimalData(AnimalData animalData)
    {
        allData.AddAnimalData(animalData);
        SaveData();
    }

    public static void RemoveAnimalData(int id)
    {
        allData.RemoveAnimalData(id);
        SaveData();
    }

    public static AnimalData GetAnimalData(int id)
    {
        return allData.GetAnimalData(id);
    }
    public static void UpdateAnimalData(int id, AnimalState state, string endTime, Vector3 position)
    {
        allData.UpdateAnimalData(id, state, endTime, position);
        SaveData();
    }
    public static void SetCurrentOrder(DeliveryOrder order)
    {
        allData.SetCurrentOrder(order);
        SaveData();
    }

    public static DeliveryOrder GetCurrentOrder()
    {
        return allData.GetCurrentOrder();
    }

    public static void SetSkipCountToday(int count)
    {
        allData.SetSkipCountToday(count);
        SaveData();
    }

    public static int GetSkipCountToday()
    {
        return allData.GetSkipCountToday();
    }

    public static void SetLastResetDate(string date)
    {
        allData.SetLastResetDate(date);
        SaveData();
    }

    public static string GetLastResetDate()
    {
        return allData.GetLastResetDate();
    }
    public static List<MissionData> GetMissions()
    {
        return allData.GetMissions();
    }

    public static void SetMissions(List<MissionData> missions)
    {
        allData.SetMissions(missions);
        SaveData();
    }

    public static List<int> GetMissionProgress()
    {
        return allData.GetMissionProgress();
    }

    public static void SetMissionProgress(List<int> progress)
    {
        allData.SetMissionProgress(progress);
        SaveData();
    }

    public static string GetNextResetTime()
    {
        return allData.GetNextResetTime();
    }

    public static void SetNextResetTime(string time)
    {
        allData.SetNextResetTime(time);
        SaveData();
    }

    public static bool GetIsBigRewardClaimed()
    {
        return allData.GetIsBigRewardClaimed();
    }

    public static void SetIsBigRewardClaimed(bool claimed)
    {
        allData.SetIsBigRewardClaimed(claimed);
        SaveData();
    }
    public static void AddListenerUpdateCoinEvent(Action updateCoin)
    {
        UpdateCoinEvent += updateCoin;
    }
    public static void RemoveListenerUpdateCoinEvent(Action updateCoin)
    {
        UpdateCoinEvent -= updateCoin;
    }
    public static void SetMusic(float volume)
    {
        allData.SetMusic(volume);
    }
    public static float GetMusic()
    {
        return allData.GetMusic();
    }
    public static void SetSound(float volume)
    {
        allData.SetSound(volume);
    }
    public static float GetSound()
    {
        return allData.GetSound();
    }
}
public class AllData
{
    public List <TileData> tileDataList;
    public List <AnimalData> animalDataList;
    public List <int> itemQuantityList;
    public List <WaitingSlot> waitingSlotList;
    public int coin;
    public int inventoryMax;
    public int totalItemQuantity;
    public float music;
    public float sound;
    public DeliveryOrder currentOrder; 
    public int skipCountToday;         
    public string lastResetDate;
    public List<MissionData> missions;
    public List<int> missionProgress;
    public string nextResetTime;
    public bool isBigRewardClaimed;
    public List<TileData> GetTileDataList()
    {
        return tileDataList;
    }
    public void AddTileData(TileData tileData)
    {       
        tileDataList.Add(tileData);
        Debug.Log(tileData.x +" + " +tileData.y);
        Debug.Log(itemQuantityList.Count);
    } 
    public void RemoveTileData(TileData tileData)
    {       
        tileDataList.Remove(tileData);
    }
    public void AddWaitingSlot(WaitingSlot waitingSlot)
    {
        waitingSlotList.Add(waitingSlot);
    } 
    public void RemoveWaitingSlot(WaitingSlot waitingSlot)
    {
        waitingSlotList.Remove(waitingSlot);
    }
    public WaitingSlot GetWaitingSlot(int id)
    {
        return waitingSlotList[id];
    }
    public int GetWaitingSlotListCount()
    {
        return waitingSlotList.Count;
    }
    public bool IsContain(int x, int y)
    {
        for (int i = 0; i < tileDataList.Count; i++)
        {
            if (tileDataList[i].x == x && tileDataList[i].y == y)
            {
                return true;
            }
        }
        return false;
    }
    public TileData GetTileDataFromList(int x, int y)
    {
        Debug.Log(x + " + " + y);
        for (int i = 0; i < tileDataList.Count; i++)
        {
            if (tileDataList[i].x == x && tileDataList[i].y == y)
            {
                Debug.Log(tileDataList[i].x + " + " + tileDataList[i].y);
                return tileDataList[i];
            }
        }
        Debug.Log("null");
        return null;
    }
    public void AddItemQuantity(int id, int quantity)
    { 
        itemQuantityList[id] += quantity;
        totalItemQuantity += quantity;
    }
    public void SubItemQuantity(int id, int quantity)
    {
        if (itemQuantityList[id] <= 0) return;
        itemQuantityList[id] -= quantity;
        totalItemQuantity -= quantity;
    }
    public int GetItemQuantity(int id)
    {
        return itemQuantityList[id]; 
    }
    public void SubCoin(int cost)
    {
        coin -= cost;
    }
    public int GetCoin()
    {
        return coin;
    }
    public bool IsEnoughMoney(int cost)
    {
        return coin >= cost;
    }
    public void AddCoin(int money)
    {
        coin += money;
    }
    public void SetInventoryMax()
    {
        inventoryMax += 50;
    }
    public int GetInventoryMax()
    {
        return inventoryMax;
    }
    public int GetTotalItemQuantity()
    {
        return totalItemQuantity;
    }
    public bool IsInventoryMax()
    {
        return totalItemQuantity >= inventoryMax;
    }
    public void SetMusic(float volume)
    {
        music = volume;
    }
    public float GetMusic()
    {
        return music;
    }
    public void SetSound(float volume)
    {
        sound = volume;
    }
    public float GetSound()
    {
        return sound;
    }
    public void AddAnimalData(AnimalData animalData)
    {
        animalDataList.Add(animalData);
    }
    
    public void RemoveAnimalData( int id) 
    {
        animalDataList.Remove(animalDataList[id]);
    }
    
    public AnimalData GetAnimalData(int id)
    {
        foreach (var animal in animalDataList)
        {
            if (animal.id == id)
            {
                return animal;
            }
        }
        return null;
    }
    public void UpdateAnimalData(int id, AnimalState state, string endTime, Vector3 position)
    {
        AnimalData animalData = GetAnimalData(id);
        if (animalData != null)
        {
            animalData.state = state;
            animalData.endTime = endTime;
            animalData.position = position;
        }
    }
    public void SetCurrentOrder(DeliveryOrder order)
    {
        currentOrder = order;      
    }

    public DeliveryOrder GetCurrentOrder()
    {
        return currentOrder;
    }

    public void SetSkipCountToday(int count)
    {
        skipCountToday = count;      
    }

    public int GetSkipCountToday()
    {
        return skipCountToday;
    }

    public void SetLastResetDate(string date)
    {
        lastResetDate = date;     
    }

    public string GetLastResetDate()
    {
        return lastResetDate;
    }
    public List<MissionData> GetMissions()
    {
        return missions;
    }

    public void SetMissions(List<MissionData> newMissions)
    {
        missions = newMissions;
        
    }

    public List<int> GetMissionProgress()
    {
        return missionProgress;
    }

    public void SetMissionProgress(List<int> progress)
    {
        missionProgress = progress;
       
    }

    public string GetNextResetTime()
    {
        return nextResetTime;
    }

    public void SetNextResetTime(string time)
    {
        nextResetTime = time;  
    }

    public bool GetIsBigRewardClaimed()
    {
        return isBigRewardClaimed;
    }

    public void SetIsBigRewardClaimed(bool claimed)
    {
        isBigRewardClaimed = claimed;
    }
}
[Serializable]
public class TileData
{
    public int x;
    public int y;
    public string endTime;
    public PlantData plantData;
    public TileData(int x, int y, string endTime, PlantData selectedPlant)
    {
        this.x = x;
        this.y = y;
        this.endTime = endTime;
        this.plantData = selectedPlant;
    }
    public DateTime GetEndTime()
    {
        return DateTime.ParseExact(endTime, "yyyy-MM-ddTHH:mm:ss", null);
    }
}
[Serializable]
public class AnimalData
{
    public int id;
    public string endTime;
    public AnimalTypeData animalTypeData;
    public AnimalState state;
    public Vector3 position;
    public AnimalData(int id, string endTime, AnimalTypeData animalTypeData, AnimalState state, Vector3 position)
    {
        this.id = id;
        this.endTime = endTime;
        this.animalTypeData = animalTypeData;
        this.state = state;
        this.position = position;
    } 
}
[Serializable]
public class TreeData
{
    public int id;
    public string treeName;
    public Vector3 position;
    public bool hasFruit;
    public float remainingTime;

    public TreeData(int id, string name, Vector3 position, int remainingTime)
    {
        this.id = id;
        this.treeName = name;
        this.position = position;
        this.hasFruit = false;
        this.remainingTime = remainingTime;
    }
}


