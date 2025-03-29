using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// UI cho hiển thị nguyên liệu
public class IngredientItem : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image itemIcon;
    [SerializeField] private UnityEngine.UI.Text itemName;
    [SerializeField] private UnityEngine.UI.Text itemQuantity;
    [SerializeField] private UnityEngine.UI.Image background;

    [SerializeField] private Color enoughColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color notEnoughColor = new Color(1, 0.7f, 0.7f, 1);

    public void Setup(Item item, int quantity, bool hasEnough)
    {
        if (item != null)
        {
            itemIcon.sprite = item.itemSprite;
            itemName.text = item.itemName;
            itemQuantity.text = "x" + quantity.ToString();

            // Đổi màu nền theo trạng thái
            background.color = hasEnough ? enoughColor : notEnoughColor;
        }
    }
}
