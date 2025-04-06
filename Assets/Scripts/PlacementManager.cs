using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    private Item currentItem;
    [SerializeField] private GameObject previewObject;
    private bool isPlacing = false;
    private SpriteRenderer previewSprite;
    public LayerMask groundLayer;
  
    private AnimalManager animalManager;

    void Start()
    {
        animalManager = FindObjectOfType<AnimalManager>();
    }

    void Update()
    {
        if (isPlacing && previewObject != null)
        {
            MovePreviewObject();
            UpdatePreviewColor();
            if (Input.GetMouseButtonDown(0) && IsValidPosition())
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CancelPlacing();
            }
        }
    }

    public void StartPlacing(Item item)
    {
        currentItem = item;
        isPlacing = true;

        if (previewObject == null)
        {
            previewObject = new GameObject("Preview");
            previewSprite = previewObject.AddComponent<SpriteRenderer>();
        }
        previewSprite.sprite = item.itemSprite;
        previewSprite.color = new Color(1, 1, 1, 0.5f); // Màu trong suốt để xem trước
        previewObject.SetActive(true);
    }

    private void PlaceObject()
    {
        Vector3 position = previewObject.transform.position;
        if (currentItem.itemType == ItemType.Animal)
        {
            animalManager.SpawnAnimalAfterPlacement(position, currentItem.id); // Spawn thú tại vị trí đã chọn
        }
        else if (currentItem.itemType == ItemType.Building)
        {
            DataPlayer.SetInventoryMax();
        }
    }

    private void CancelPlacing()
    {
        if (previewObject != null)
        {
            previewObject.SetActive(false);
        }
        isPlacing = false;
    }

    private void MovePreviewObject()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        previewObject.transform.position = mousePos;
    }
    private void UpdatePreviewColor()
    {
        previewSprite.color = IsValidPosition()
            ? new Color(1, 1, 1, 0.5f) // Trong suốt khi hợp lệ
            : new Color(1, 0, 0, 0.5f); // Đỏ khi không hợp lệ
    }
    private bool IsValidPosition()
    {
        // Kiểm tra vị trí hợp lệ (có thể tùy chỉnh thêm)
        Collider2D hit = Physics2D.OverlapPoint(previewObject.transform.position, groundLayer);
        return hit != null; // Đảm bảo vị trí nằm trên groundLayer
    }
}
