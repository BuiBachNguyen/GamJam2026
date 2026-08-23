using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlipPicture : MonoBehaviour
{
    [SerializeField] private float spinSpeed = 360f; // Đã tăng tốc độ để xoay 1 vòng/giây
    [SerializeField] private Image image;
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;

    [Header("Gán Object TopHalf vào đây")]
    [SerializeField] private Image topHalfImage;

    private float totalRotatedAngle = 0f; // Biến theo dõi tổng góc đã xoay
    private bool hasFinishedSpinning = false; // Đánh dấu đã xoay xong chưa

    private string notify = "[Phía sau tấm ảnh có hiện dòng chữ: 2408]";
    public GameObject panel;

    private void Start()
    {
        
    }

    public void SetUp(Image img)
    {
        topHalfImage = img;
    }

    private void Update()
    {
        // Nếu đã xoay xong rồi thì không làm gì nữa, kết thúc hàm tại đây
        if (hasFinishedSpinning) return;

        // Tính toán góc sẽ xoay trong frame này
        float step = spinSpeed * Time.deltaTime;

        // Cập nhật tổng góc đã xoay
        totalRotatedAngle += step;

        // Xoay object
        transform.Rotate(0f, step, 0f);

        // Lấy góc Y hiện tại để quyết định hiện mặt trước hay sau
        float angle = transform.eulerAngles.y;
        if (image != null)
        {
            image.sprite = angle > 90f && angle < 270f ? backSprite : frontSprite;
        }

        // KIỂM TRA ĐIỀU KIỆN DỪNG: Nếu tổng góc xoay vượt quá hoặc bằng 360 độ (1 vòng)
        if (totalRotatedAngle >= 360f)
        {
            hasFinishedSpinning = true;

            // 1. Ép góc xoay về đúng 0 độ để tránh bị lệch (do frame rate)
            transform.localEulerAngles = Vector3.zero;
            if (image != null) image.sprite = frontSprite;

            // 2. Gán FrontSprite vào thẳng TopHalf
            if (topHalfImage != null)
            {
                topHalfImage.sprite = frontSprite;
                GameLivingroomController.instance.StartCoroutine(notifyProcess());
            }

            // 3. Tùy chọn: Tắt luôn object FlipPicture này đi vì đã hoàn thành nhiệm vụ
            this.gameObject.SetActive(false);
        }

        IEnumerator notifyProcess()
        {
            NotificationUIController.instance.setContent(notify, 2f);

            yield return new WaitForSeconds(2.5f);

            panel.SetActive(false);

            // co the them doan hoi thoai nho o day

        }
    }
}