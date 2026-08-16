using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Inventory
{
    public int id;
    public int amount;
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
    List<Inventory> listInventory;
    #endregion
    private void Awake()
    {
        MakeSingleTon();
    }

    private void Start()
    {
        InitData();
    }

    #region Function
    void InitData()
    {
        listInventory = new List<Inventory>();
    }

    public void saveInventory(Inventory inventory)
    {
        foreach (Inventory item in listInventory)
        {
            if (item.id == inventory.id)
            {
                item.amount += inventory.amount;
                return;
            }
        }
        listInventory.Add(inventory);
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
    #endregion
}
