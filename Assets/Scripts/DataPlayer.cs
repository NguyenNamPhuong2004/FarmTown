using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataPlayer 
{
    private const string ALL_DATA = "all_data";
    public static AllData allData;
    static DataPlayer()
    {
        allData = JsonUtility.FromJson<AllData>(PlayerPrefs.GetString(ALL_DATA));
        if (allData == null)
        {
            allData = new AllData
            {
                tileDataList = new List<TileData>(),
                animalDataList = new List<AnimalData>(),
                itemQuantityList = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                waitingSlotList = new List<WaitingSlot>(),
                music = 0.5f,
                sound = 0.5f
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
    public static void AddAnimalData(AnimalData animalData)
    {
        allData.AddAnimalData(animalData);
        SaveData();
    }

    public static void RemoveAnimalData(AnimalData animalData)
    {
        allData.RemoveAnimalData(animalData);
        SaveData();
    }

    public static AnimalData GetAnimalData(int id)
    {
        return allData.GetAnimalData(id);
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
    public float music;
    public float sound;
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
    }
    public void SubItemQuantity(int id, int quantity)
    {
        if (itemQuantityList[id] <= 0) return;
        itemQuantityList[id] -= quantity;
    }
    public int GetItemQuantity(int id)
    {
        return itemQuantityList[id]; 
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
    
    public void RemoveAnimalData(AnimalData animalData) 
    {  
        animalDataList.Remove(animalData);
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


