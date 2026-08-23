using System.Collections;
using UnityEditor;
using UnityEngine;

public class BrotherFile : File
{
    public Dialog dialog;
    public float timeWait = 0.5f;
    public override void Click()
    {
        base.Click();

        StartCoroutine(showDialog());
    }


     
    IEnumerator showDialog()
    {
        yield return new WaitForSeconds(timeWait);

        DialogController.instance.playDialog(dialog);
    }
}
