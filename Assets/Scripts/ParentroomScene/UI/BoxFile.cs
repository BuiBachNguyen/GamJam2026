using System.Collections;
using UnityEngine;

public class BoxFile : File
{
    public Dialog dialog;
    public float timeWait = 0.5f;
    public FadeTransition fadeTransition;
    public override void Click()
    {
        base.Click();

        GameParentroomController.instance.StartCoroutine(showDialog());
    }



    IEnumerator showDialog()
    {
        yield return new WaitForSeconds(timeWait);

        DialogController.instance.playDialog(dialog, () =>
        {
            PlayerPrefs.SetInt("Computer", 1); // luu de khong phai xai may tinh nua 
                                               // tat UI

            StartCoroutine(fade());
            

        });
    }

    IEnumerator fade()
    {
        fadeTransition.Appear();

        yield return new WaitForSeconds(1.5f);

        EventController.canInteractWithLocker?.Invoke(true);

        PCUIController.instance.showPCPanel(false);

        SystemControl.instance.addAction();

        fadeTransition.Fade();
    }
}
