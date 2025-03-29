using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class PlantGrowth : MonoBehaviour
{
    public Tilemap plantTilemap;           
    public TileBase seedlingTile;           
    public TileBase youngPlantTile;         
    public TileBase maturePlantTile;        
    public float growthTime = 5f;          

    private void Start()
    {
        StartCoroutine(GrowPlant());
    }

    IEnumerator GrowPlant()
    {
        Vector3Int position = plantTilemap.WorldToCell(transform.position);

       
        plantTilemap.SetTile(position, seedlingTile);
        yield return new WaitForSeconds(growthTime);

        
        plantTilemap.SetTile(position, youngPlantTile);
        yield return new WaitForSeconds(growthTime);

    
        plantTilemap.SetTile(position, maturePlantTile);
    }
}
