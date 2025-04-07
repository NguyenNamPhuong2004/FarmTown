using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlantingSystem : MonoBehaviour
{
    public Tilemap soilTilemap;
    public Tilemap plantTilemap;
    public TileBase soilTile; 

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
            if (plantTilemap.GetTile(gridPosition) == null )
            {
                if (soilTilemap.GetTile(gridPosition) == soilTile)
                {
                    if (selectedPlantType != null && selectedPlantType.plantCost <= DataPlayer.GetCoin())
                    {
                        plantTilemap.SetTile(gridPosition, selectedPlantType.seedlingTile);
                        StartTime = DateTime.Now;
                        EndTime = StartTime.AddSeconds((double)selectedPlantType.growthTime);
                        TileData tileData = new TileData(gridPosition.x, gridPosition.y, EndTime.ToString("yyyy-MM-ddTHH:mm:ss"), selectedPlantType);
                        DataPlayer.AddTileData(tileData);
                        DataPlayer.SubCoin(selectedPlantType.plantCost);
                    }
                    else
                    {
                        plantTypesUI.SetActive(true);
                    }
                }
            }
            else
            {
                TileData tileData = null;
                // Kiểm tra xem có tìm thấy dữ liệu tile không
                if (DataPlayer.IsContain(gridPosition.x, gridPosition.y))
                {
                    tileData = DataPlayer.GetTileDataFromList(gridPosition.x, gridPosition.y);
                }

                if (tileData != null)
                {
                    if (plantTilemap.GetTile(gridPosition) == tileData.plantData.maturePlantTile)
                    {
                        // Xóa tile trước
                        plantTilemap.SetTile(gridPosition, null);

                        try
                        {
                            // Thêm vào inventory - đây là nơi có thể phát sinh lỗi
                            inventory.AddItem(tileData.plantData.plantName, 1);
                        }
                        catch (Exception e)
                        {
                            Debug.LogError("Lỗi khi thêm vào inventory: " + e.Message);
                        }

                        // Kiểm tra xem đây có phải là selectedPlant không
                        if (selectedPlant != null && selectedPlant.x == gridPosition.x && selectedPlant.y == gridPosition.y)
                        {
                            selectedPlant = null;
                            plantNameUI.text = "";
                            plantGrowthTimeUI.text = "00:00:00";
                        }

                        // Cuối cùng là xóa khỏi danh sách
                        DataPlayer.RemoveTileData(tileData);
                    }
                    else
                    {
                        plantNameUI.text = tileData.plantData.plantName;
                        selectedPlant = tileData;
                    }
                }
            }
        }

        if (selectedPlant != null)
        {
            try
            {
                DateTime start = DateTime.Now;
                EndTime = DateTime.ParseExact(selectedPlant.endTime, "yyyy-MM-ddTHH:mm:ss", null);
                timeleft = EndTime - start;
                int second = Mathf.FloorToInt((float)(timeleft.TotalSeconds)) % 60;
                int minute = Mathf.FloorToInt((float)(timeleft.TotalMinutes)) % 60;
                int hour = Mathf.FloorToInt((float)(timeleft.TotalHours));

                if (timeleft.TotalSeconds <= 0)
                    plantGrowthTimeUI.text = string.Format("{0:00}:{1:00}:{2:00}", 0, 0, 0);
                else
                    plantGrowthTimeUI.text = string.Format("{0:00}:{1:00}:{2:00}", hour, minute, second);
            }
            catch (Exception)
            {
                selectedPlant = null;
                plantGrowthTimeUI.text = "00:00:00";
            }
        }
        else
        {
            plantGrowthTimeUI.text = "00:00:00";
        }
    }

    private void FixedUpdate()
    {
        List<TileData> tileList = DataPlayer.GetTileDataList();
        if (tileList == null || tileList.Count == 0) return;

        for (int i = 0; i < tileList.Count; i++)
        {
            TileData tile = tileList[i];
            if (tile == null) continue;

            try
            {
                DateTime start = DateTime.Now;
                DateTime endTime = DateTime.ParseExact(tile.endTime, "yyyy-MM-ddTHH:mm:ss", null);
                TimeSpan timeLeft = endTime - start;
                Vector3Int tilePosition = new Vector3Int(tile.x, tile.y, 0);

                if (timeLeft.TotalSeconds < 0)
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
            catch (Exception e)
            {
                Debug.LogError("Lỗi khi cập nhật cây: " + e.Message);
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

