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
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero); // Kiểm tra tất cả collider tại vị trí chuột
            foreach (var hit in hits)
            {
                if (hit.collider != null)
                {
                    Debug.Log("Hit: " + hit.collider.gameObject.name);
                    AppleTree tree = hit.collider.GetComponent<AppleTree>();
                    if (tree != null)
                    {
                        int treeId = activeTrees.IndexOf(tree.gameObject);
                        if (treeId >= 0)
                        {
                            Debug.Log("Tree ID: " + treeId);
                            OnTreeClicked(treeId);
                            break; // Chỉ xử lý cây đầu tiên được nhấp
                        }
                    }
                }
            }
            if (hits.Length == 0)
            {
                Debug.Log("No collider hit at " + mousePos);
            }
        }
    }
    public void SpawnTreeAfterPlacement(Vector3 position)
    {
        int newId = activeTrees.Count;
        TreeData treeData = new TreeData(newId, "AppleTree", position, 1200);

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
        }
        else
        {
            string timeRemaining = tree.GetRemainingTimeFormatted();
            Debug.Log($"Cây: {tree.TreeName}, Thời gian còn lại: {timeRemaining}");
        }
    }
}