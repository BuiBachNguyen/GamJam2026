using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Box : PickableItem
{

    public Sprite close, open;
    SpriteRenderer sp;
    public Dialog dialog;
    public Dialog AfterOpenDialog;
    public GameObject BoxPanel;

    public FadeTransition fadeTransition;
    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }
    public override void Start()
    {
        if (PlayerPrefs.GetInt("ExistBox") == 1)
        {
            this.gameObject.SetActive(true);
        } else
        {
            gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Box") == 1)
        {
            sp.sprite = open;
            canShowTuto = false;
        }
        else if (PlayerPrefs.GetInt("Box") == 0)
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
        StartCoroutine(fade());
        PlayerPrefs.SetInt("Box", 1); // Đừng quên lưu lại data nhé!
    }

    IEnumerator fade()
    {
        fadeTransition.Appear();

        yield return new WaitForSeconds(1.5f);

        BoxPanel.SetActive(true);

        SystemControl.instance.addAction();

        fadeTransition.Fade();

        //SystemControl.instance.addAction();

        yield return new WaitForSeconds(1.5f);

        DialogController.instance.playDialog(AfterOpenDialog , () =>
        {
            BoxPanel.GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(() =>
            {
                BoxPanel.SetActive(false);
                SystemControl.instance.removeAction();
                bool itemExist = InventorySystem.instance.saveInventory(new Inventory(id, 1));
                if (!itemExist)
                {
                    InventoryItem info = InventorySystem.instance.addInventoryToUI(id);
                    NotificationUIController.instance.setContent("[Bạn nhận được " + info.inventoryName + ".]", timeWait);
                }
            });
        }) ;
    }
}
