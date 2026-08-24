using System.Collections;
using UnityEngine;

public class LivingroomDoor : Door
{
    public string notifyString = "[Phòng đã bị khóa, cần có chìa khóa để mở.]";
    public float timeWait = 1.5f;
    public Dialog dialogInfo;
    public override void OnInteract()
    {
        TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
        // kiem tra dieu kien
        if (InventorySystem.instance.getInventory(KeyData.KeyLivingroom) != null)
        {
            base.OnInteract();
            return;
        }
        NotificationUIController.instance.setContent(notifyString, timeWait);
        // chay mot doan hoi thoai nho
        StartCoroutine(conversation());
    }

    IEnumerator conversation()
    {
        yield return new WaitForSeconds(timeWait + 1f);

        DialogController.instance.playDialog(dialogInfo);
    }
}
