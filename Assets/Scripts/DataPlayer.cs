using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                animalTimeList = new List<AnimalTime>(),
                itemQuantityList = new List<int>(8) 
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
}
public class AllData
{
    public List <TileData> tileDataList;
    public List <AnimalTime> animalTimeList;
    public List <int> itemQuantityList;

    public List<TileData> GetTileDataList()
    {
        return tileDataList;
    }
    public void AddTileData(TileData tileData)
    {       
        tileDataList.Add(tileData);
    } 
    public void RemoveTileData(TileData tileData)
    {       
        tileDataList.Remove(tileData);
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
        for (int i = 0; i < tileDataList.Count; i++)
        {
            if (tileDataList[i].x == x && tileDataList[i].y == y)
            {
                return tileDataList[i];
            }
        }
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
public class AnimalTime
{
    public string endTime;
    public bool isTime;
    public AnimalData animalData;
    public AnimalTime(string endTime, bool isTime, AnimalData selectedAnimal)
    {
        this.endTime = endTime;
        this.isTime = isTime;
        this.animalData = selectedAnimal;
    }
}

