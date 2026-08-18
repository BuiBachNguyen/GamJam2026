using System.Collections.Generic;
using UnityEngine;



public class ItemSlotPooling : MonoBehaviour
{
    public static ItemSlotPooling instance;

    void MakeSingleton()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleton();
    }

    #region Variable
    public GameObject slotItemPrefab;
    public int slotNumber = 40;
    #endregion

    // Hàng đợi chứa các object đang không được sử dụng
    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    private void Start()
    {
        // Khởi tạo sẵn số lượng object đưa vào pool
        for (int i = 0; i < slotNumber; i++)
        {
            GameObject obj = Instantiate(slotItemPrefab);
            obj.SetActive(false); // Tắt đi chờ gọi
            poolQueue.Enqueue(obj);
        }
    }

    // Lấy một GameObject ra khỏi Pool
    public GameObject GetSlot()
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true); // Bật lên trước khi giao đi
            return obj;
        }

        // Nếu Pool đã cạn (xài lố 40 cái), sinh thêm cái mới
        GameObject newObj = Instantiate(slotItemPrefab);
        newObj.SetActive(true);
        return newObj;
    }

    // Trả GameObject về lại Pool khi không xài nữa
    public void ReturnSlot(GameObject obj)
    {
        obj.SetActive(false); // Tắt đi
        poolQueue.Enqueue(obj); // Cất lại vào hàng đợi
    }
}

