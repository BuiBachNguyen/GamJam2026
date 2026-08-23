using DG.Tweening;
using System.Collections;
using UnityEngine;

public class DataChecker : MonoBehaviour
{
    [SerializeField] private string correctCode = "1234";
    [SerializeField] private GenericInput inputSystem;

    public float timeWait = 1f;
    public void ValidateCode(string enteredCode)
    {
        Debug.Log(enteredCode);
        if (enteredCode == correctCode)
        {
            PCUIController.instance.ClosePassPanel();
            PCUIController.instance.showOpenFolder(true);
            PCUIController.instance.OpenFolderPanel.GetComponent<CanvasGroup>().DOFade(1f, 1f);
        }
        else
        {
            inputSystem.ClearInput();
            StartCoroutine(notify());
        }
    }

    IEnumerator notify()
    {
        PCUIController.instance.setNotifyText("Sai mật khẩu");

        yield return new WaitForSeconds(timeWait);

        PCUIController.instance.setNotifyText("");
    }

}
