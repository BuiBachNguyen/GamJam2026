using UnityEngine;
using UnityEngine.UI;

public class PictureWindow : MonoBehaviour
{

    Image content;

    private void Awake()
    {
        content = GetComponentInChildren<Image>();
    }
    public void SetUp(Sprite img)
    {
        content.sprite = img;
    }
}
