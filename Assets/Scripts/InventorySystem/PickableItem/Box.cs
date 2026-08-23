using UnityEngine;

public class Box : PickableItem
{

    public Sprite close, open;
    SpriteRenderer sp;
    public Dialog dialog;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }
    public override void Start()
    {
        if (PlayerPrefs.GetInt("Box") == 1)
        {
            sp.sprite = open;
            canShowTuto = false;
        } else if (PlayerPrefs.GetInt("Box") == 0)
        {
            sp.sprite = close;
        }
    }

    public override void PickUpProcess(Collision2D collision)
    {
        if (havePicked) return;
        if (!player.IsInteract) return;

        // Tiêu hao nút tương tác để tránh kẹt nút
        player.IsInteract = false;

        if (PlayerPrefs.GetInt("Box") == 1)
        {
            return;
        }

        TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);

        // NẾU CHƯA CÓ KÉO
        if (InventorySystem.instance.getInventory(KeyData.Scissor) == null)
        {
            DialogController.instance.playDialog(dialog);
            // Return luôn, KHÔNG set havePicked = true ở đây để còn quay lại bấm lần 2
            return;
        }

        // NẾU CÓ KÉO -> MỞ HỘP THÀNH CÔNG
        havePicked = true; // Lúc này mới đánh dấu là đã dùng xong
        sp.sprite = open;
        PlayerPrefs.SetInt("Box", 1); // Đừng quên lưu lại data nhé!
    }
}
