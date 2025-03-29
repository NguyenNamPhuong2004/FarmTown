using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AnimalState
{
    Hungry,
    Full,
    Product
}
public class Animal : MonoBehaviour
{
    public AnimalData animalData;
    public AnimalState animalState;

    DateTime StartTime;
    DateTime EndTime;
    bool isTime;
    TimeSpan timeleft;

    private void Awake()
    {
        animalState = AnimalState.Hungry;
    }

    private void OnMouseDown()
    {
        if(animalState == AnimalState.Hungry)
        {

        }
    }
}
