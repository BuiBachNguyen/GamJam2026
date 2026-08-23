using System.Collections;
using UnityEngine;

public class InsideLocker : PickableItem
{
    bool canInteract;
    public Dialog dialog;
    public FadeTransition fadeTransition;
    public GameObject playerLocker, cameraLocker;
    public GameObject box;
    private void OnEnable()
    {
        canInteract = false;
        EventController.canInteractWithLocker += useBox;
    }

    private void OnDisable()
    {
        EventController.canInteractWithLocker -= useBox;
    }

    public override void Start()
    {
        if (PlayerPrefs.GetInt("Locker") == 1)
        {
            GetComponent<Collider2D>().enabled = false;
        }
        GetComponent<Collider2D>().enabled = false;
    }

    public override void PickUpProcess(Collider2D collision)
    {
        if (havePicked) return;
        if (!player.IsInteract) return;
        havePicked = true;
        player.IsInteract = false;
        if (!canInteract) return;
        canInteract = false;
        if (PlayerPrefs.GetInt("Locker") == 1)
        {
            GetComponent<Collider2D>().enabled = false;
            return;
        }
        TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
        GetComponent<Collider2D>().enabled = false;
        DialogController.instance.playDialog(dialog, () => afterInteract());
    }

    public void useBox(bool can)
    {
        canInteract = can;
        GetComponent<Collider2D>().enabled = can;
    }

    void afterInteract()
    {
        bool itemExist = InventorySystem.instance.saveInventory(new Inventory(id, 1));
        if (!itemExist)
        {
            InventoryItem info = InventorySystem.instance.addInventoryToUI(id);
            NotificationUIController.instance.setContent("[Bạn nhận được " + info.inventoryName + ".]", timeWait);
            StartCoroutine(fade());
        }
    }

    IEnumerator fade()
    {
        yield return new WaitForSeconds(timeWait + 1f);

        fadeTransition.Appear();

        yield return new WaitForSeconds(1.5f);

        player.transform.position = playerLocker.transform.position;    

        Camera.main.transform.position = cameraLocker.transform.position;   

        box.SetActive(true);

        fadeTransition.Fade();

        yield return new WaitForSeconds(1.5f);

        SystemControl.instance.addAction();
    }
}
