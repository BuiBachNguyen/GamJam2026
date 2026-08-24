using System.Collections;
using UnityEngine;

public class KeyLivingroom : PickableItem
{
    public Dialog dialogInfo;

    public override void PickUpProcess(Collider2D collision)
    {
        if (havePicked) return;
        if (!player.IsInteract) return;
        if (canBePickedUpUnder && player.Fsm.currentState is not PickUpState) // tranh spam nut
        {
            player.Fsm.ChangeState(new PickUpState());
            timeDestroy = 0.5f;
        }
        havePicked = true;
        player.IsInteract = false;
        bool itemExist = InventorySystem.instance.saveInventory(new Inventory(id, 1));
        if (!itemExist)
        {
            InventoryItem info = InventorySystem.instance.addInventoryToUI(id);
            NotificationUIController.instance.setContent("[Bạn nhận được " + info.inventoryName + ".]", timeWait);
            GameMainBedroomController.instance.StartCoroutine(conversation());
        }
        PlayerPrefs.SetInt("Object" + id, 1);
    }


    IEnumerator conversation()
    {
        yield return new WaitForSeconds(timeDestroy);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(timeWait + 1.5f);

        DialogController.instance.playDialog(dialogInfo, () =>
        {
            Destroy(gameObject);
        });

    }
}
