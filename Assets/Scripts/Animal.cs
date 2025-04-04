using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public int Id { get; private set; }
    public AnimalState State { get; private set; }
    public AnimalTypeData AnimalTypeData { get; private set; }
    public string EndTimeString { get; private set; }

    private SpriteRenderer spriteRenderer;
    private DateTime endTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
    }

    public void Initialize(AnimalData animalData)
    {
        Id = animalData.id;
        State = animalData.state;
        AnimalTypeData = animalData.animalTypeData;
        EndTimeString = animalData.endTime;

        if (!string.IsNullOrEmpty(EndTimeString))
        {
            DateTime.TryParse(EndTimeString, out endTime);
        }

        spriteRenderer.sprite = animalData.animalTypeData.animalSprite;
        CheckProductionState();
    }

    private void Update()
    {
        if (State == AnimalState.Full)
        {
            CheckProductionState();
        }
    }

    private void CheckProductionState()
    {
        if (State == AnimalState.Full && !string.IsNullOrEmpty(EndTimeString))
        {
            if (DateTime.Now >= endTime)
            {
                State = AnimalState.Product;
                AnimalManager.Instance.ClearProductionInfo();
            }
        }
    }

    public void Feed()
    {
        State = AnimalState.Full;
        endTime = DateTime.Now.AddMinutes(AnimalTypeData.produceTime);
        EndTimeString = endTime.ToString();
    }

    public void ResetToHungry()
    {
        State = AnimalState.Hungry;
        EndTimeString = "";
    }

    public TimeSpan GetRemainingTime()
    {
        if (State != AnimalState.Full || string.IsNullOrEmpty(EndTimeString))
            return TimeSpan.Zero;

        TimeSpan remaining = endTime - DateTime.Now;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    private void OnMouseDown()
    {
        AnimalManager.Instance.OnAnimalClicked(Id);
    }
}
