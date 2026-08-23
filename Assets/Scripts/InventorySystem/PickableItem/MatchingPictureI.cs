using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MatchingPicture : MonoBehaviour
{
    public bool isMatched = false;
    public int idToCheck = 6;
    public GameObject fullPicture;

    // THÊM BIẾN NÀY ĐỂ LẤY THÔNG TIN TỪ CUỐN SÁCH
    public Book book;

    public void Update()
    {
        if (InventorySystem.instance == null) return;
        if (InventorySystem.instance.getInventory(idToCheck) == null) return;
        if (InventorySystem.instance.getInventory(idToCheck).amount <= 0) return;

        // THÊM DÒNG NÀY: Nếu chưa lật tới trang cuối thì bỏ qua, không làm gì hết
        if (book != null && book.currentPage < book.TotalPageCount) return;

        Debug.Log("Final");
        if (!isMatched)
        {
            isMatched = true; // Đánh dấu là đã giải đố xong (để các code khác biết)
            fullPicture.SetActive(true); // Bật hình lên
            //fullPicture.GetComponent<FlipPicture>().SetUp(GetComponentInChildren<Image>());
        }
    }
}
