using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeepUIFocus : MonoBehaviour
{
    private GameObject lastSelectedObject;

    // Hàm này tự động được Unity gọi khi game bị mất hoặc nhận lại tiêu điểm (như Alt-Tab)
    private void OnApplicationFocus(bool hasFocus)
    {
        if (EventSystem.current == null) return;

        if (!hasFocus) // Khi Tab ra ngoài
        {
            // Lưu lại nút đang được chọn trước khi tab ra (nếu có)
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                lastSelectedObject = EventSystem.current.currentSelectedGameObject;
            }
        }
        else // Khi Tab ngược lại vào game
        {
            // Kiểm tra xem nút cũ còn tồn tại và đang bật không
            if (lastSelectedObject != null && lastSelectedObject.activeInHierarchy)
            {
                // Bắt buộc phải đợi 1 frame rồi mới gán lại, nếu không EventSystem sẽ đè lên
                StartCoroutine(ReselectNextFrame(lastSelectedObject));
            }
        }
    }

    private IEnumerator ReselectNextFrame(GameObject objectToSelect)
    {
        yield return null; // Đợi hết frame hiện tại

        if (EventSystem.current != null)
        {
            // Mẹo: Set null trước rồi mới set lại object để ép EventSystem cập nhật highlight
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(objectToSelect);
        }
    }
}
