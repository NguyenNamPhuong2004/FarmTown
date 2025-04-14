using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private Transform missionContainer;
    [SerializeField] private GameObject missionPrefab;
    [SerializeField] private Text timeLeftText;

    private MissionSystem missionSystem;

    private void Start()
    {
        missionSystem = MissionSystem.Instance;
        missionSystem.OnMissionsUpdated += UpdateUI;
        UpdateUI();
    }

    private void Update()
    {
        TimeSpan timeLeft = missionSystem.GetTimeLeft();
        timeLeftText.text = $"Time left: {timeLeft.Hours:00}:{timeLeft.Minutes:00}:{timeLeft.Seconds:00}";
    }

    private void UpdateUI()
    {
        foreach (Transform child in missionContainer)
        {
            Destroy(child.gameObject);
        }

        List<MissionData> missions = missionSystem.GetCurrentMissions();
        List<int> progress = DataPlayer.GetMissionProgress();

        for (int i = 0; i < missions.Count; i++)
        {
            GameObject ui = Instantiate(missionPrefab, missionContainer);
            ui.transform.Find("Description").GetComponent<Text>().text = missions[i].description;
            ui.transform.Find("ProgressBar").Find("ProgressText").GetComponent<Text>().text =
                progress[i] == -1 ? "Completed" : $"{progress[i]}/{missions[i].requiredAmount}";
            Image itemImage = ui.transform.Find("ItemSprite").GetComponent<Image>();
            itemImage.sprite = missions[i].itemSprite;
            itemImage.enabled = missions[i].itemSprite != null;
        }
    }
}