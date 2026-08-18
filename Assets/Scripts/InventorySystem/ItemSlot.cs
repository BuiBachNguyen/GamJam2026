using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private string itemDescription;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemCount;
    private Image itemAva;
    private GameObject chooseOutline;


    public void SetUp(string itemName, string itemCount, Sprite itemAva, string itemDescription)
    {
        foreach (Transform t in this.transform)
        {
            switch (t.name)
            {
                case "Choose":
                    chooseOutline = t.gameObject;
                    break;
                case "ItemAva":
                    this.itemAva = t.GetComponent<Image>();
                    break;
                case "ItemName":
                    this.itemName = t.GetComponent<TextMeshProUGUI>();
                    break;
                case "ItemCount":
                    this.itemCount = t.GetComponent<TextMeshProUGUI>();
                    break;
            }
        }
        this.itemAva.sprite = itemAva;
        this.itemName.text = itemName;  
        this.itemCount.text = itemCount;
        this.itemDescription = itemDescription;
    }

    public void highlightOutline(bool state)
    {
        chooseOutline.SetActive(state);
    }

    public void OnSelect(BaseEventData eventData)
    {
        highlightOutline(true);
        InventoryUIController.instance.setContentText(this.itemDescription);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        highlightOutline(false);
    }

    private void OnDisable()
    {
        highlightOutline(false);
    }
}
