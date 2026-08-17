using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[System.Serializable]   
public class Item
{
    public int id;
    public InventoryItem item;
}

public class InventoryDictionary : MonoBehaviour
{
    #region SingleTon
    public static InventoryDictionary instance;


    void MakeSingleTon()
    {
        if (instance == null || instance != this)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private void Awake()
    {
        MakeSingleTon();
    }

    private void Start()
    {
        InitData();
    }

    // ======== Dictionary ========
    [SerializeField] List<Item> listItem;
    Dictionary<int, InventoryItem> dictionary;


    void InitData()
    {
        dictionary = new Dictionary<int, InventoryItem>();
        foreach (Item item in listItem)
        {
            dictionary.Add(item.id, item.item);
        }
    }

    public InventoryItem getInventorySO(int id)
    {
        return dictionary.ContainsKey(id) ? dictionary[id]:null;
    }

}
