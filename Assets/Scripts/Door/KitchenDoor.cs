using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class KitchenDoor : Door
{
    public float timeWait = 1.5f;
    public string notifyString = "[Phòng đã bị khóa, cần có chìa khóa để mở.]";
    public Dialog dialog;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetTrigger("Close");
    }

    public override void OnInteract()
    {
        TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
        if (InventorySystem.instance.getInventory(KeyData.KeyKitchen) != null)
        {
            StartCoroutine(openDoor());
            return;
        }
        NotificationUIController.instance.setContent(notifyString, timeWait);
            StartCoroutine(conversation());
    }

    IEnumerator conversation()
    {
        yield return new WaitForSeconds(timeWait + 1f);

        DialogController.instance.playDialog(dialog);
    }

    IEnumerator openDoor()
    {
        animator.SetTrigger("Open");

        yield return null;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        base.OnInteract();

        animator.SetTrigger("Close");

    }
}
