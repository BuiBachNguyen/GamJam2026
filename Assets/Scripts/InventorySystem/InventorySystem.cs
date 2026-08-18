using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory
{
    public int id;
    public int amount;

    public Inventory(int id, int amount)
    {
        this.id = id;
        this.amount = amount;
    }
}

public class InventorySystem : MonoBehaviour
{
    #region SingleTon
    public static InventorySystem instance;

    
    void MakeSingleTon()
    {
        if (instance == null || instance != this)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        } else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Variable
    public List<Inventory> listInventory;

    private GridLayoutGroup gridLayout;
    #endregion
    private void Awake()
    {
        MakeSingleTon();
    }

    private void Start()
    {
        InitData();
        LoadData();
    }

    #region Function
    void InitData()
    {
        listInventory = new List<Inventory>();
        gridLayout = InventoryUIController.instance.gridLayout;
    }

    public bool saveInventory(Inventory inventory)
    {
        foreach (Inventory item in listInventory)
        {
            if (item.id == inventory.id)
            {
                item.amount += inventory.amount;
                return true; // neu item da ton tai
            }
        }
        listInventory.Add(inventory);
        Debug.Log(listInventory.Count);
        return false;
    }

    public Inventory getInventory(int id)
    {
        return listInventory.Find(x => x.id == id);
    }

    // hàm này chưa giải quyết được vấn đề nếu 2 vật có chung id
    public void removeInventory(int id)
    {
        listInventory.Remove(listInventory.Find(x => x.id == id));
    }

    public void clearInventory()
    {
        listInventory.Clear();
    }

    public void LoadData()
    {
        foreach (Inventory item in listInventory)
        {
            addInventoryToUI(item.id);
        }
    }

    public void addInventoryToUI(int id)
    {
        InventoryItem infoItem = InventoryDictionary.instance.getInventorySO(id);
        if (infoItem != null)
        {
            GameObject itemPrefab = InventoryUIController.instance.slotItemPrefab;
            GameObject realSlot = Instantiate(itemPrefab, gridLayout.transform);
            realSlot.transform.SetParent(gridLayout.transform, false);
            // set up dữ liệu
            realSlot.GetComponent<ItemSlot>().SetUp(infoItem.inventoryName, ": 1", infoItem.inventoryImage, infoItem.inventoryDescription);
        }
    }
    #endregion
}
