using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TreeManager : MonoBehaviour
{
    public static TreeManager Instance;
    public GameObject treePrefab;
    public Transform treeParent;
    public List<GameObject> activeTrees = new List<GameObject>();
    public InventorySystem inventorySystem;
    private ProductionInfo productionInfo;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        productionInfo = FindObjectOfType<ProductionInfo>();
        if (productionInfo == null)
        {
            Debug.LogError("ProductionInfo not found in the scene!");
        }
    }

    private void Start()
    {
        LoadTrees();
    }

    private void LoadTrees()
    {
        activeTrees.Clear();
        foreach (var treeData in DataPlayer.GetTreeDataList())
        {
            SpawnTree(treeData);
        }
    }

    public void SpawnTreeAfterPlacement(Vector3 position)
    {
        int newId = DataPlayer.GetTreeDataList().Count;
        TreeData treeData = new TreeData(newId, "Apple Tree", position, 420); 
        DataPlayer.AddTreeData(treeData);
        SpawnTree(treeData);
    }

    private void SpawnTree(TreeData treeData)
    {
        GameObject treeObj = Instantiate(treePrefab, treeData.position, Quaternion.identity, treeParent);
        AppleTree treeComponent = treeObj.GetComponent<AppleTree>();

        treeComponent.Initialize(treeData);
        activeTrees.Add(treeObj);
    }

    public void OnTreeClicked(int treeId, bool isRightClick = false)
    {
        if (treeId < 0 || treeId >= activeTrees.Count)
            return;

        AppleTree tree = activeTrees[treeId].GetComponent<AppleTree>();

        if (isRightClick)
        {
            SellTree(treeId);
        }
        else
        {
            switch (tree.State)
            {
                case TreeState.Growing:
                    ShowGrowthProgress(tree);
                    break;
                case TreeState.HasFruit:
                    CollectFruit(tree);
                    break;
            }
        }
    }

    private void ShowGrowthProgress(AppleTree tree)
    {
        if (productionInfo != null)
        {
            productionInfo.ShowProductionInfo(tree.Id, tree.TreeName, tree.GetRemainingTime(), "Tree");
        }
    }

    private void CollectFruit(AppleTree tree)
    {
        inventorySystem.AddItem("Apple", 1);
        tree.ResetToGrowing();
        MissionSystem.Instance.TrackProgress("Apple", 1, MissionType.Harvest);
        Debug.Log("Đã thu hoạch 1 quả táo!");
        SoundManager.Ins.Harvest();
    }

    private void SellTree(int treeId)
    {
        if (treeId < 0 || treeId >= activeTrees.Count)
            return;

        GameObject treeObj = activeTrees[treeId];
        Destroy(treeObj);

        activeTrees.RemoveAt(treeId);
        DataPlayer.RemoveTreeData(treeId);

        for (int i = 0; i < activeTrees.Count; i++)
        {
            AppleTree tree = activeTrees[i].GetComponent<AppleTree>();
            tree.UpdateId(i);
            DataPlayer.GetTreeDataList()[i].id = i;
        }

        if (productionInfo != null)
        {
            productionInfo.ClearProductionInfo();
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null)
            {
                AppleTree tree = hit.GetComponent<AppleTree>();
                if (tree != null)
                {
                    int treeId = activeTrees.IndexOf(tree.gameObject);
                    if (treeId >= 0)
                    {
                        OnTreeClicked(treeId, false);
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(mousePos);
            if (hit != null)
            {
                AppleTree tree = hit.GetComponent<AppleTree>();
                if (tree != null)
                {
                    int treeId = activeTrees.IndexOf(tree.gameObject);
                    if (treeId >= 0)
                    {
                        OnTreeClicked(treeId, true);
                    }
                }
            }
        }
    }
}