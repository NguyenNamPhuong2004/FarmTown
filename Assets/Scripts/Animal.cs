using System;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public int Id { get; private set; }
    public AnimalState State { get; private set; }
    public AnimalTypeData AnimalTypeData { get; private set; }
    public string EndTimeString { get; private set; }

    private SpriteRenderer spriteRenderer;
    private DateTime endTime;
    private ProductionInfo productionInfo;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        }
        productionInfo = FindObjectOfType<ProductionInfo>(); 
        if (productionInfo == null)
        {
            Debug.LogError("ProductionInfo not found in the scene!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject == gameObject)
            {
                AnimalManager.Instance.OnAnimalClicked(Id, false);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null && hit.gameObject == gameObject)
            {
                AnimalManager.Instance.OnAnimalClicked(Id, true);
            }
        }

        CheckProductionState();
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

    private void CheckProductionState()
    {
        if (State == AnimalState.Full && !string.IsNullOrEmpty(EndTimeString))
        {
            TimeSpan remainingTime = endTime - DateTime.Now;
            if (remainingTime <= TimeSpan.Zero)
            {
                State = AnimalState.Product;
                EndTimeString = "";
                if (productionInfo != null)
                {
                    productionInfo.ClearProductionInfo(); 
                }
                DataPlayer.UpdateAnimalData(Id, State, EndTimeString, transform.position); 
                Debug.Log($"{AnimalTypeData.animalName} đã sẵn sàng để thu hoạch!");
            }
        }
    }

    public void Feed()
    {
        State = AnimalState.Full;
        endTime = DateTime.Now.AddSeconds(AnimalTypeData.produceTime);
        EndTimeString = endTime.ToString("yyyy-MM-ddTHH:mm:ss"); 
    }

    public void ResetToHungry()
    {
        State = AnimalState.Hungry;
        EndTimeString = "";
    }

    public TimeSpan GetRemainingTime()
    {
        if (State != AnimalState.Full || string.IsNullOrEmpty(EndTimeString))
        {
            return TimeSpan.Zero;
        }

        TimeSpan remaining = endTime - DateTime.Now;
        return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
    }

    public void UpdateId(int newId)
    {
        Id = newId;
    }
}