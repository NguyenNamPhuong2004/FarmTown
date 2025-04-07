using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewPlantData", menuName = "Farm/Plant Data")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public int plantCost;
    public TileBase seedlingTile;
    public TileBase youngPlantTile;
    public TileBase maturePlantTile;
    public float growthTime;
}
