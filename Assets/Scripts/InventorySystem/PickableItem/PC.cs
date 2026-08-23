using System.Collections;
using Unity.AppUI.UI;
using UnityEngine;

public class PC : PickableItem
{
    public Dialog alreadyUsePCDialog;
    public Dialog usePCDialog;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Start()
    {
        if (PlayerPrefs.GetInt("Computer") == 1)
        {
            GetComponent<Collider2D>().enabled = false; // ngan khong cho tuong tac 
        }
    }

    public override void PickUpProcess(Collider2D collision)
    {
        if (havePicked) return;
        if (!player.IsInteract) return;
        havePicked = true;
        player.IsInteract = false;
        if (PlayerPrefs.GetInt("Computer") == 1)
        {
            // da xong thu thach 
            DialogController.instance.playDialog(alreadyUsePCDialog, () =>
            {
                GetComponent<Collider2D>().enabled = false;
            });
            return;
        }
        TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
        DialogController.instance.playDialog(usePCDialog, () =>
        {
            StartCoroutine(openPC());
        });
    }

    IEnumerator openPC()
    {
        animator.SetTrigger("Open");

        yield return null;

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);

        // mo panel va reset PC ve dong 
        animator.SetTrigger("Close");
        havePicked = false;
        PCUIController.instance.showPCPanel(true);
    }
}
