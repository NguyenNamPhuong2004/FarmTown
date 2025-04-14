using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlantingSystem : MonoBehaviour
{
    public Tilemap soilTilemap;
    public Tilemap plantTilemap;
    public TileBase soilTile; 

    public PlantData selectedPlantType;
    public GameObject plantTypes;
    public TileData selectedPlant;
    public Text plantNameUI;
    public Text plantGrowthTimeUI;

    public InventorySystem inventory;
   
    void Update()
    {
        HandleMouseInput();
        UpdateSelectedPlantUI();
    }

    private void FixedUpdate()
    {
        UpdateAllTrees();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = soilTilemap.WorldToCell(mousePosition);

            if (soilTilemap.GetTile(gridPosition) == soilTile)
            {
                if (plantTilemap.GetTile(gridPosition) == null)
                {
                    if (selectedPlantType != null && selectedPlantType.plantCost <= DataPlayer.GetCoin())
                    {
                        PlantTree(gridPosition);
                    }
                    else
                    {
                        GameUIManager.Ins.OpenPlantType();
                    }
                }
                else
                {
                    HandleTreeInteraction(gridPosition);
                }
            }
            //else
            //{
            //    selectedPlant = null;
            //}
        }
    }

    private void PlantTree(Vector3Int gridPosition)
    {
        plantTilemap.SetTile(gridPosition, selectedPlantType.seedlingTile);
        DateTime endTime = DateTime.Now.AddSeconds(selectedPlantType.growthTime);
        string endTimeString = endTime.ToString("yyyy-MM-ddTHH:mm:ss"); 
        TileData tileData = new TileData(gridPosition.x, gridPosition.y, endTimeString, selectedPlantType);
        DataPlayer.AddTileData(tileData);
        DataPlayer.SubCoin(selectedPlantType.plantCost);
        MissionSystem.Instance.TrackProgress(selectedPlantType.plantName, 1, MissionType.Plant);
        SoundManager.Ins.Plant();
    }

    private void HandleTreeInteraction(Vector3Int gridPosition)
    {
        TileData tileData = DataPlayer.GetTileDataFromList(gridPosition.x, gridPosition.y);
        if (tileData != null)
        {
            DateTime endTime = tileData.GetEndTime();
            TimeSpan timeLeft = endTime - DateTime.Now;
            if (timeLeft.TotalSeconds <= 0 && plantTilemap.GetTile(gridPosition) == tileData.plantData.maturePlantTile)
            {
                HarvestTree(gridPosition, tileData);
            }
            else
            {
                SelectTree(tileData);
            }
        }
    }

    private void HarvestTree(Vector3Int gridPosition, TileData tileData)
    {
        plantTilemap.SetTile(gridPosition, null);
        inventory.AddItem(tileData.plantData.plantName, 1);
        MissionSystem.Instance.TrackProgress(tileData.plantData.plantName, 1, MissionType.Harvest);
        if (selectedPlant == tileData)
        {
            selectedPlant = null;
            plantNameUI.text = "";
            plantGrowthTimeUI.text = "00:00:00";
        }
        DataPlayer.RemoveTileData(tileData);
        SoundManager.Ins.Haverst();
    }

    private void SelectTree(TileData tileData)
    {
        selectedPlant = tileData;
        plantNameUI.text = tileData.plantData.plantName;
    }

    private void UpdateSelectedPlantUI()
    {
        if (selectedPlant != null)
        {
            DateTime endTime = selectedPlant.GetEndTime();
            TimeSpan timeLeft = endTime - DateTime.Now;
            if (timeLeft.TotalSeconds <= 0)
            {
                plantGrowthTimeUI.text = "00:00:00";
            }
            else
            {
                int hours = (int)timeLeft.TotalHours;
                int minutes = timeLeft.Minutes;
                int seconds = timeLeft.Seconds;
                plantGrowthTimeUI.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
            }
        }
    }

    private void UpdateAllTrees()
    {
        List<TileData> tileList = DataPlayer.GetTileDataList();
        if (tileList == null || tileList.Count == 0) return;

        DateTime now = DateTime.Now;
        foreach (TileData tile in tileList)
        {
            Vector3Int tilePosition = new Vector3Int(tile.x, tile.y, 0);
            DateTime endTime = tile.GetEndTime();
            TimeSpan timeLeft = endTime - now;

            if (timeLeft.TotalSeconds <= 0)
            {
                plantTilemap.SetTile(tilePosition, tile.plantData.maturePlantTile);
            }
            else if (timeLeft.TotalSeconds < tile.plantData.growthTime / 2)
            {
                plantTilemap.SetTile(tilePosition, tile.plantData.youngPlantTile);
            }
            else
            {
                plantTilemap.SetTile(tilePosition, tile.plantData.seedlingTile);
            }
        }
    }

    public void SelectPlant(PlantData plant)
    {
        selectedPlantType = plant;
    }
    public int CountPlantedTrees(string plantName)
    {
        int count = 0;
        List<TileData> tileList = DataPlayer.GetTileDataList();
        if (tileList != null)
        {
            foreach (TileData tile in tileList)
            {
                if (tile.plantData.plantName == plantName)
                    count++;
            }
        }
        return count;
    }
}

