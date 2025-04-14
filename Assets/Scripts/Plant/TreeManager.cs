using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    public static TreeManager Instance;
    public GameObject treePrefab; 
    public Transform treeParent;
    public List<GameObject> activeTrees = new List<GameObject>();
    public InventorySystem inventorySystem;
    private PlacementManager placementManager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        placementManager = FindObjectOfType<PlacementManager>();
        if (placementManager == null) Debug.LogError("PlacementManager not found!");
    }
   
    public void SpawnTreeAfterPlacement(Vector3 position)
    {
        int newId = activeTrees.Count;
        TreeData treeData = new TreeData(newId, "AppleTree", position, 420);

        GameObject treeObj = Instantiate(treePrefab, position, Quaternion.identity, treeParent);
        AppleTree treeComponent = treeObj.GetComponent<AppleTree>(); 
        treeComponent.Initialize(treeData);

        activeTrees.Add(treeObj);
    }

    public void OnTreeClicked(int treeId)
    {
        if (treeId < 0 || treeId >= activeTrees.Count) return;

        AppleTree tree = activeTrees[treeId].GetComponent<AppleTree>(); 
        if (tree.HasFruit)
        {
            inventorySystem.AddItem("Apple", 1);
            tree.ResetToGrowing();
            Debug.Log("Đã thu hoạch 1 quả táo!");
            SoundManager.Ins.Haverst();
        }
        else
        {
            string timeRemaining = tree.GetRemainingTimeFormatted();
            Debug.Log($"Cây: {tree.TreeName}, Thời gian còn lại: {timeRemaining}");
        }
    }
}