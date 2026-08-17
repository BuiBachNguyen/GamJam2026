using UnityEngine;

public class PickableItem : MonoBehaviour
{
    public int id;
    private bool havePicked = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollider"))
        {
            PlayerController player = collision.gameObject.GetComponentInParent<PlayerController>();
            if (collision.gameObject.GetComponentInParent<PlayerController>().Fsm.currentState is not PickUpState || havePicked) return;
            Debug.Log("Is Picking Item");
            havePicked = true;
            InventorySystem.instance.saveInventory(new Inventory(id, 1));
            Destroy(gameObject, 0.5f); // tà đạo
        }
    }
}
