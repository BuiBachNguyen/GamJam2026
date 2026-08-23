using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class File : MonoBehaviour, IPointerClickHandler
{
    Button btn;
    public GameObject WindowPrefab;
    public Sprite contentImg;
    public Canvas canvas;
    public RectTransform drag;
    private void Awake()
    {
        btn = GetComponent<Button>();
    }


    public virtual void Click()
    {
        GameObject pictureWindow = Instantiate(WindowPrefab);
        pictureWindow.transform.SetParent(transform.parent, false);
        // truyen du lieu 
        pictureWindow.GetComponentInChildren<PictureWindow>().SetUp(contentImg);
        pictureWindow.GetComponentInChildren<TitleBar>().SetUp(canvas, drag);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 2)
        {
            Click();
        }
    }
}
