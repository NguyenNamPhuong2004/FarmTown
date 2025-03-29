using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public int id;
    public Item item;
    public GameObject slot;
    public int quantity;
    public Text quantityText;

    public void AddItem(int quantityAdd)
    {
        DataPlayer.AddItemQuantity(id, quantityAdd);
        quantity = DataPlayer.GetItemQuantity(id);
        quantityText.text = quantity.ToString();
        if (quantity > 0) AddSlot();
    }
    public void SubItem(int quantitySub)
    {
        DataPlayer.SubItemQuantity(id, quantitySub);
        quantity = DataPlayer.GetItemQuantity(id);
        quantityText.text = quantity.ToString();
        if (quantity == 0)
        {
            ClearSlot();
        }
    }
    public void AddSlot()
    {
        slot.SetActive(true);
    }
    public void ClearSlot()
    {
        slot.SetActive(false);
    }
}