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

    public void UpdateDisplay(string animalName, TimeSpan remainingTime)
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

    public void ClearDisplay()
    {
        if (animalNameText != null) animalNameText.text = "";
        if (timeText != null) timeText.text = "00:00:00:00";
    }


}
