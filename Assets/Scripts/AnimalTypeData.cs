using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewAnimalData", menuName = "Farm/Animal Data")]
public class AnimalTypeData : ScriptableObject
{
    public string animalName;
    public float produceTime;
    public string productName;
    public Sprite animalSprite;
    public Sprite productSprite;
}
