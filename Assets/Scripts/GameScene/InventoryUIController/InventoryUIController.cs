using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


// Class này dùng để quản lý mọi thứ liên quan đến UI túi đồ
public class InventoryUIController : MonoBehaviour
{
    #region SingleTon
    public static InventoryUIController instance;

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

    private void Awake()
    {
        MakeSingleTon();
    }

    #region Component

    [Header ("TEXT")]
    public TextMeshProUGUI contentText;

    [Header ("LAYOUT")]
    public GridLayoutGroup gridLayout;


    [Header ("PANEL/ Image")]
    public GameObject inventoryPanel;

    [Header("PREFABS")]
    public GameObject slotItemPrefab;

    #endregion

    public bool isShowingInventoryPanel()
    {
        return inventoryPanel.activeSelf;
    }
    public void showInventoryPanel(bool state)
    {
        inventoryPanel.SetActive(state);
        if (state ) FocusFirstItem();
    }

    public void setContentText(string txt)
    {
        contentText.text = txt;
    }

    public void addGridComponent(GameObject gameObject)
    {
        gameObject.transform.SetParent(gridLayout.transform, false);
    }

    private void FocusFirstItem()
    {
        EventSystem.current.SetSelectedGameObject(null);
        foreach (Transform child in gridLayout.transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(child.gameObject);
                return;
            }
        }
        Debug.Log("Túi đồ trống, không có gì để Focus!");
    }


}
