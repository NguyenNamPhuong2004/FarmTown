using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionInfoUI : MonoBehaviour
{
    public Text animalNameText;
    public Text timeText;

    private void Awake()
    {
        if (animalNameText == null || timeText == null)
        {
            Debug.LogError("Please assign animalNameText and timeText in the Inspector for ProductionInfoUI!");
        }
    }
    private void Start()
    {
        ProductionInfo.OnUpdateProductionUI += HandleUpdateProductionUI;
        ProductionInfo.OnClearProductionUI += HandleClearProductionUI;
    }
    private void OnEnable()
    {
        ProductionInfo.OnUpdateProductionUI += HandleUpdateProductionUI;
        ProductionInfo.OnClearProductionUI += HandleClearProductionUI;
    }

    private void OnDisable()
    {
        ProductionInfo.OnUpdateProductionUI -= HandleUpdateProductionUI;
        ProductionInfo.OnClearProductionUI -= HandleClearProductionUI;
    }

    private void HandleUpdateProductionUI(string animalName, TimeSpan remainingTime)
    {
        UpdateDisplay(animalName, remainingTime);
    }

    private void HandleClearProductionUI()
    {
        ClearDisplay();
    }

    private void UpdateDisplay(string animalName, TimeSpan remainingTime)
    {
        Debug.Log(remainingTime);
        if (animalNameText != null)
            animalNameText.text = animalName;
        Debug.Log(1);

        if (timeText != null)
        {
            Debug.Log(2);
            timeText.text = string.Format("{0:00}:{1:00}:{2:00}",
                   remainingTime.Hours,
                   remainingTime.Minutes,
                   remainingTime.Seconds);
            Debug.Log("1"+timeText.text);
        }
        else Debug.Log(3);
    }

    private void ClearDisplay()
    {
        if (animalNameText != null) animalNameText.text = "";
        if (timeText != null) timeText.text = "00:00:01";
    }


}
