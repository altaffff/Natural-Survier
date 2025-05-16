using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum InventoryItems
{
    key,
    timeIncreaser,
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [System.Serializable]
    public class InventoryClass
    {
        public InventoryItems inventoryItems;
        public int itemQuantity;
        public GameObject uiSlot;           
        public TextMeshProUGUI quantityText; 
    }

    public List<InventoryClass> inventory = new List<InventoryClass>();

    public void AddItems(InventoryItems items)
    {
        InventoryClass result = inventory.Find(item => item.inventoryItems == items);

        if (result != null)
        {
            result.itemQuantity++;

            if (result.itemQuantity == 1)
            {
                if (result.uiSlot != null)
                    result.uiSlot.SetActive(true);
                if (result.quantityText != null)
                    result.quantityText.gameObject.SetActive(true);
            }

            UpdateItemUI(result);
        }
        else
        {
            Debug.LogWarning("Tried to add item not initialized in the Inventory list.");
        }
    }


    public bool HasItem(InventoryItems item)
    {
        InventoryClass found = inventory.Find(i => i.inventoryItems == item && i.itemQuantity > 0);
        return found != null;
    }

    public void UseItem(InventoryItems item)
    {
        InventoryClass found = inventory.Find(i => i.inventoryItems == item && i.itemQuantity > 0);
        if (found != null)
        {
            found.itemQuantity = Mathf.Max(0, found.itemQuantity - 1);
            UpdateItemUI(found);
        }
    }

    private void UpdateItemUI(InventoryClass item)
    {
        if (item.quantityText != null)
        {
            item.quantityText.text = item.itemQuantity.ToString();
        }
    }

    public void UpdateInventoryUI()
    {
        foreach (var item in inventory)
        {
            UpdateItemUI(item);
        }
    }
}
