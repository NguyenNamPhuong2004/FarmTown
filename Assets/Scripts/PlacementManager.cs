using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    private Item currentItem;
    private GameObject previewObject;
    private bool isPlacing = false;
    private int remainingQuantity;
    private SpriteRenderer previewSprite;

    public LayerMask groundLayer; // Layer mặt đất
    public LayerMask obstacleLayer; // Layer công trình, vật nuôi, cây trồng
    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void Update()
    {
        if (isPlacing && previewObject != null)
        {
            MovePreviewObject();
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

    public void StartPlacing(Item item, int quantity)
    {
        currentItem = item;
        remainingQuantity = quantity;
        isPlacing = true;

        // Tạo object preview 2D
        previewObject = new GameObject("Preview");
        previewSprite = previewObject.AddComponent<SpriteRenderer>();
        previewSprite.sprite = item.itemSprite;
        previewSprite.color = new Color(1, 1, 1, 0.5f); // Trong suốt 50%
        previewObject.AddComponent<BoxCollider2D>(); // Thêm collider để kiểm tra
    }

    private void MovePreviewObject()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        previewObject.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    private bool IsValidPosition()
    {
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(
            previewObject.transform.position,
            previewObject.GetComponent<BoxCollider2D>().size,
            0,
            obstacleLayer
        );

        if (overlaps.Length > 0)
        {
            Debug.Log("Vị trí không hợp lệ: Đè lên vật thể khác!");
            return false;
        }

        Collider2D groundHit = Physics2D.OverlapPoint(previewObject.transform.position, groundLayer);
        if (groundHit == null)
        {
            Debug.Log("Vị trí không hợp lệ: Không nằm trên mặt đất!");
            return false;
        }

        return true;
    }

    private void PlaceObject()
    {
        remainingQuantity--;
        Debug.Log($"Đã đặt {currentItem.itemName} (ID: {currentItem.id}) tại {previewObject.transform.position}");

        if (currentItem.itemType == ItemType.Animal)
        {
            DataPlayer.AddAnimalData(new AnimalData(
                DataPlayer.allData.animalDataList.Count,
                "",
                null, 
                AnimalState.Hungry,
                previewObject.transform.position
            ));
        }
        else if (currentItem.itemType == ItemType.Building)
        {
            // Lưu dữ liệu công trình nếu cần, ví dụ vào tileDataList hoặc danh sách riêng
            Debug.Log("Đã đặt công trình, bạn có thể xử lý thêm logic lưu trữ ở đây.");
        }

        if (remainingQuantity > 0)
        {
            previewObject = new GameObject("Preview");
            previewSprite = previewObject.AddComponent<SpriteRenderer>();
            previewSprite.sprite = currentItem.itemSprite;
            previewSprite.color = new Color(1, 1, 1, 0.5f);
            previewObject.AddComponent<BoxCollider2D>();
        }
        else
        {
            Destroy(previewObject);
            isPlacing = false;
            previewObject = null;
        }
    }

    private void CancelPlacing()
    {
        Destroy(previewObject);
        isPlacing = false;
        previewObject = null;
        Debug.Log("Đã hủy đặt item!");
    }
}