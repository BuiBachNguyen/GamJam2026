using DG.Tweening;
using System.Collections;
using UnityEngine;

public class BrotherDoor : Door
{
    public string notifyString = "[Ở đây có một lỗ nhỏ, có thể tra thứ gì đó vào được.]";
    public string afterString = "[Đã sử dụng tay nắm cửa.]";
    public float timeWait = 2f;
    public Dialog dialog;
    public Dialog AfterDialog;
    public Dialog FinalDialog;
    public Dialog ParentDialog;
    public GameObject blurCanvas;
    public GameObject picture;
    public GameObject TheEndPanel;

    public override void OnInteract()
    {
        TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
        if (InventorySystem.instance.getInventory(KeyData.DoorNumb) != null)
        {
            NotificationUIController.instance.setContent(afterString, timeWait);

            StartCoroutine(process());
        } else
        {
            NotificationUIController.instance.setContent(notifyString, timeWait);
            StartCoroutine(conversation());
        }
    }

    IEnumerator process()
    {
        yield return new WaitForSeconds(timeWait + 0.5f);

        base.OnInteract();

        SystemControl.instance.addAction();

        yield return new WaitForSeconds(2f);

        AudioManager.Instance.StopBGM();

        blurCanvas.SetActive(true);

        DialogController.instance.playDialog(AfterDialog, () =>
        {
            picture.SetActive(true);
            picture.GetComponent<CanvasGroup>().alpha = 0f;
            picture.GetComponent<CanvasGroup>().DOFade(1f, 1f).OnComplete(() =>
            {
                DialogController.instance.playDialog(FinalDialog, () =>
                {
                    DialogController.instance.playDialog(ParentDialog, () =>
                    {
                        TheEndPanel.SetActive(true);
                    });
                });
            });
        });
    }

    IEnumerator conversation()
    {
        yield return new WaitForSeconds(timeWait + 0.5f);

        DialogController.instance.playDialog(dialog);
    }
}
