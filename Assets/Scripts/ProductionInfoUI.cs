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
        if (animalNameText != null)
            animalNameText.text = animalName;

        if (timeText != null)
        {
            timeText.text = string.Format("{0:00}:{1:00}:{2:00}:{3:00}",
                remainingTime.Days,
                remainingTime.Hours,
                remainingTime.Minutes,
                remainingTime.Seconds);
        }
    }

    private void ClearDisplay()
    {
        if (animalNameText != null) animalNameText.text = "";
        if (timeText != null) timeText.text = "00:00:00:00";
    }


}
