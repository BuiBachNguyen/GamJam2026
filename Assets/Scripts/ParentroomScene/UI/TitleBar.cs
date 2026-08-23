using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class TitleBar : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    [Tooltip("Kéo RectTransform của Object Cha (Window) vào đây")]
    [SerializeField] private RectTransform windowToMove;
    [SerializeField] private RectTransform dragArea;

    Button closeBtn;

    private Canvas canvas;

    public void SetUp(Canvas canvas, RectTransform drag)
    {
        this.canvas = canvas;
        dragArea = drag;
        closeBtn = GetComponentInChildren<Button>();
        closeBtn.onClick.RemoveAllListeners();
        closeBtn.onClick.AddListener(() => Destroy(this.transform.parent.gameObject));
    }

    private void Awake()
    {
        if (windowToMove == null)
        {
            windowToMove = transform.parent.GetComponent<RectTransform>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (windowToMove != null)
        {
            windowToMove.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (windowToMove != null && canvas != null && dragArea != null)
        {
            Vector2 newPosition = windowToMove.anchoredPosition + (eventData.delta / canvas.scaleFactor);

            float minX = dragArea.rect.min.x - windowToMove.rect.min.x;
            float maxX = dragArea.rect.max.x - windowToMove.rect.max.x;
            float minY = dragArea.rect.min.y - windowToMove.rect.min.y;
            float maxY = dragArea.rect.max.y - windowToMove.rect.max.y;

            newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

            windowToMove.anchoredPosition = newPosition;
        }
    }
}