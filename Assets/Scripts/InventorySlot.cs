using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Item item;
    public GameObject slot;
    public int quantity;
    private void Start()
    {
        quantity = DataPlayer.GetItemQuantity(item.id);
    }
    public void AddItem(int quantityAdd)
    {
        DataPlayer.AddItemQuantity(item.id, quantityAdd);
        quantity = DataPlayer.GetItemQuantity(item.id);
        
        if (quantity > 0) AddSlot();
    }
    public void SubItem(int quantitySub)
    {
        DataPlayer.SubItemQuantity(item.id, quantitySub);
        quantity = DataPlayer.GetItemQuantity(item.id);
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