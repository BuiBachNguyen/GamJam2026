using System.Collections;
using UnityEngine;

public class BrotherDoor : Door
{
    public string notifyString = "[Ở đây có một lỗ nhỏ, có thể tra thứ gì đó vào được.]";
    public string afterString = "[Đã sử dụng tay nắm cửa.]";
    public float timeWait = 2f;
    public Dialog dialog;

    public override void OnInteract()
    {
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
    }

    IEnumerator conversation()
    {
        yield return new WaitForSeconds(timeWait + 0.5f);

        DialogController.instance.playDialog(dialog);
    }
}
