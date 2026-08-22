using UnityEngine;
using UnityEngine.UI;

public class FlipPicture : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 180f;
    [SerializeField] private Image image;
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;

    private void Update()
    {
        transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f);

        float angle = transform.eulerAngles.y;

        if(image == null )
            image = this.gameObject.GetComponentInChildren<Image>();
        if(image != null ) 
        image.sprite = angle > 90f && angle < 270f
            ? backSprite
            : frontSprite;
 
        if (Input.GetKeyDown(KeyCode.Q))
            Destroy(this);
    }
}