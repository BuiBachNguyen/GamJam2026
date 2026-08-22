using UnityEngine;

public class LivingroomDoor : Door
{
    public string notifyString = "[Phòng đã bị khóa, cần có chìa khóa để mở.]";
    public float timeWait = 1.5f;
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
    }
}
