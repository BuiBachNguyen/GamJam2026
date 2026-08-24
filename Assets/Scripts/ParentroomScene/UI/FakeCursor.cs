using UnityEngine;

public class FakeCursor : MonoBehaviour
{
    public RectTransform customCursor;   // Con trỏ giả
    public RectTransform boundaryPanel;  // Vùng giới hạn

    // Canvas của bạn là Overlay hay Camera? (Thường mặc định là null cho Overlay)
    public Camera canvasCamera;

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Chuyển đổi chuột (Pixel) sang Tọa độ Local của Panel
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            boundaryPanel,
            Input.mousePosition,
            canvasCamera,
            out Vector2 localMousePos
        );

        // 2. Lấy giới hạn thật sự của Panel (bất chấp việc kéo giãn)
        Rect rect = boundaryPanel.rect;
        float minX = rect.xMin;
        float maxX = rect.xMax;
        float minY = rect.yMin;
        float maxY = rect.yMax;

        // 3. Ép giới hạn
        localMousePos.x = Mathf.Clamp(localMousePos.x, minX, maxX);
        localMousePos.y = Mathf.Clamp(localMousePos.y, minY, maxY);

        // 4. Gán vị trí cho con trỏ giả (vì nó là con của Panel nên dùng localPosition)
        customCursor.localPosition = localMousePos;
    }
}
