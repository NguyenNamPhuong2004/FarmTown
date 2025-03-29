using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class PlantingSystem : MonoBehaviour
{
    public Tilemap soilTilemap;
    public Tilemap plantTilemap;
    public TileBase soilTile;
    public List<PlantData> plantTypes;  

    private Camera mainCamera;
    public PlantData selectedPlantType;
    public TileData selectedPlant;
    public GameObject plantTypesUI;
    public Text plantNameUI;
    public Text plantGrowthTimeUI;

    DateTime StartTime;
    DateTime EndTime;
    bool isTime;
    TimeSpan timeleft;

    public InventorySystem inventory;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = soilTilemap.WorldToCell(mousePosition);
            if (plantTilemap.GetTile(gridPosition) == null)
            {
                if (soilTilemap.GetTile(gridPosition) == soilTile)
                {
                    if (selectedPlantType != null)
                    {
                        plantTilemap.SetTile(gridPosition, selectedPlantType.seedlingTile);
                        StartTime = DateTime.Now;
                        EndTime = StartTime.AddSeconds((double)selectedPlantType.growthTime);
                        TileData tileData = new TileData(gridPosition.x, gridPosition.y, EndTime.ToString("yyyy-MM-ddTHH:mm:ss"), selectedPlantType);
                        DataPlayer.AddTileData(tileData);
                    }
                    else
                    {
                        plantTypesUI.SetActive(true);
                    }
                }
            }
            else 
            {
                if (plantTilemap.GetTile(gridPosition) == DataPlayer.GetTileDataFromList(gridPosition.x, gridPosition.y).plantData.maturePlantTile)
                {
                    plantTilemap.SetTile(gridPosition, null);
                    DataPlayer.RemoveTileData(DataPlayer.GetTileDataFromList(gridPosition.x, gridPosition.y));
                    inventory.AddItem(DataPlayer.GetTileDataFromList(gridPosition.x, gridPosition.y).plantData.plantName, 1);
                }
                else
                {
                    plantNameUI.text = DataPlayer.GetTileDataFromList(gridPosition.x, gridPosition.y).plantData.plantName;
                    selectedPlant = DataPlayer.GetTileDataFromList(gridPosition.x, gridPosition.y);
                }
            }
        }
        DateTime start = DateTime.Now;
        EndTime = DateTime.ParseExact(selectedPlant.endTime, "yyyy-MM-ddTHH:mm:ss", null);
        timeleft = EndTime - start;
        Vector3Int tilePosition = new Vector3Int(selectedPlant.x, selectedPlant.y, 0);
        int second = Mathf.FloorToInt((float)(timeleft.TotalSeconds)) % 60;
        int minute = Mathf.FloorToInt((float)(timeleft.TotalMinutes)) % 60;
        int hour = Mathf.FloorToInt((float)(timeleft.TotalHours));
        if (timeleft.TotalSeconds <= 0) plantGrowthTimeUI.text = string.Format("{0:00}:{1:00}:{2:00}",0, 0, 0);
        else plantGrowthTimeUI.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
    }
    private void FixedUpdate()
    {
        if (DataPlayer.GetTileDataList() == null) return;
        foreach (TileData tile in DataPlayer.GetTileDataList())
        {
            DateTime start = DateTime.Now;
            EndTime = DateTime.ParseExact(tile.endTime, "yyyy-MM-ddTHH:mm:ss", null);
            timeleft = EndTime - start;
            Vector3Int tilePosition = new Vector3Int(tile.x, tile.y, 0);
            int second = Mathf.FloorToInt((float)(timeleft.TotalSeconds)) % 60;
            int minute = Mathf.FloorToInt((float)(timeleft.TotalMinutes)) % 60;
            int hour = Mathf.FloorToInt((float)(timeleft.TotalHours));
            
            if (timeleft.TotalSeconds < 0)
            {
                plantTilemap.SetTile(tilePosition, tile.plantData.maturePlantTile);
            }
            else if (timeleft.TotalSeconds < tile.plantData.growthTime / 2)
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
    
  /*  private void Awake()
    {
        EndTime = DataPlayer.GetEndtime();
        DateTime start = DateTime.Now;
        timeleft = EndTime - start;
        if (DataPlayer.GetisTime() == true)
        {
            //TimeDaily.SetActive(true);
           // DailyoffBtn.SetActive(true);
            int second = Mathf.FloorToInt((float)(timeleft.TotalSeconds)) % 60;
            int minute = Mathf.FloorToInt((float)(timeleft.TotalMinutes)) % 60;
            int hour = Mathf.FloorToInt((float)(timeleft.TotalHours));
           // TimeDaily.GetComponent<Text>().text = string.Format("Return back: {0:00}:{1:00}:{2:00}", hour, minute, second);
        }
        else
        {
           // TimeDaily.SetActive(false);
           // DailyoffBtn.SetActive(false);
           // DataPlayer.SetCanDaily(true);
        }
    }
    private void FixedUpdate()
    {
        if (DataPlayer.GetisTime() == true)
        {
            DateTime start = DateTime.Now;
            EndTime = DataPlayer.GetEndtime();
            timeleft = EndTime - start;
            if (timeleft.TotalSeconds < 0)
            {
              //  TimeDaily.SetActive(false);
               // DailyoffBtn.SetActive(false);
                DataPlayer.SetisTime(isTime);
              //  DataPlayer.SetCanDaily(true);
            }
            else
            {
                int second = Mathf.FloorToInt((float)(timeleft.TotalSeconds)) % 60;
                int minute = Mathf.FloorToInt((float)(timeleft.TotalMinutes)) % 60;
                int hour = Mathf.FloorToInt((float)(timeleft.TotalHours));
               // TimeDaily.GetComponent<Text>().text = string.Format("Return back : {0:00}:{1:00}:{2:00}", hour, minute, second);

            }
        }
    }

    private void HandleTime()
    {
        StartTime = DateTime.Now;
        EndTime = StartTime.AddHours(24);
        DataPlayer.SetEndtime(EndTime);
        DataPlayer.SetisTime(isTime);
       // TimeDaily.SetActive(true);
       // DailyoffBtn.SetActive(true);
       // DataPlayer.SetCanDaily(false);
    }*/
 }

