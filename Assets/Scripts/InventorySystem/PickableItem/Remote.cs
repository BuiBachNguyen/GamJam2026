using System.Collections;
using UnityEngine;

public class Remote : PickableItem
{
    string tutorialString = "*Sử dụng R để mở tầm nhìn camera và WASD để di chuyển*";

    private void OnEnable()
    {
        PlayerController.IsRemoteUsed += UseRemote;
    }

    private void OnDisable()
    {
        PlayerController.IsRemoteUsed -= UseRemote;
    }
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
        bool itemExist = InventorySystem.instance.saveInventory(new Inventory(id, 1));
        if (!itemExist)
        {
            InventoryItem info = InventorySystem.instance.addInventoryToUI(id);
            NotificationUIController.instance.setContent("[Bạn nhận được " + info.inventoryName + ".]", timeWait);
            NotificationUIController.instance.setTutorial(tutorialString);
            TutorialManager.instance.StartCoroutine(tutorial());
        }
        //Destroy(gameObject, timeDestroy);
    }

    IEnumerator tutorial()
    {
        yield return new WaitForSeconds(timeDestroy);

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;


        yield return new WaitForSeconds(timeWait + 0.2f);

        SystemControl.instance.forceAllowSwitchMode = true;

        SystemControl.instance.addAction();

        TutorialManager.instance.ShowTutorialSwitchMode(true, player.transform.position);


    }

    void UseRemote(bool used)
    {
        if (used)
        {
            SystemControl.instance.forceAllowSwitchMode = false;
            TutorialManager.instance.ShowTutorialSwitchMode(false, Vector3.zero);
            SystemControl.instance.removeAction();
            PlayerPrefs.SetInt("Object" + id, 1);
            Destroy(gameObject);
        }
    }
}
