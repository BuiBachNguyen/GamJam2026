using System.Collections;
using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public int id;
    protected bool havePicked = false;
    public bool canBePickedUpUnder = false; // dung de chay animation pick up
    protected PlayerController player;

    protected float timeDestroy = 0f;

    public float timeWait = 2f;

    public virtual void Start()
    {
        if (PlayerPrefs.GetInt("Object" + id) == 1)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            Debug.Log("Colliding");
            player = collision.GetComponentInParent<PlayerController>();
            showPickUpTutorial(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            
            showPickUpTutorial(false);
            //player = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            PickUpProcess(collision);
        }
    }

    public void showPickUpTutorial(bool state)
    {
        if (player != null)
        {
            TutorialManager.instance.ShowTutorialInteraction(state, player.transform.position);
            Debug.Log("Tutorial");
        }
        else
        {
            Debug.Log("Player null");
        }

    }

    public virtual void PickUpProcess(Collider2D collision)
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
        }
        PlayerPrefs.SetInt("Object" + id, 1);
        Destroy(gameObject, timeDestroy);
    }
}
