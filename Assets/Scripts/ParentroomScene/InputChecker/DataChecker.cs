using System.Collections;
using UnityEngine;

public class DataChecker : MonoBehaviour
{
    [SerializeField] private string correctCode = "1234";
    [SerializeField] private GenericInput inputSystem;

    public float timeWait = 1f;
    public void ValidateCode(string enteredCode)
    {
        if (enteredCode == correctCode)
        {
            Debug.Log("Correct Pass");
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
