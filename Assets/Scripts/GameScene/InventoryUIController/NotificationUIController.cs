using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationUIController : MonoBehaviour
{
    public static NotificationUIController instance;

    void MakeSingleton()
    {
        if (instance == null || instance != this)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleton();
    }

    public TextMeshProUGUI contentText;
    public TextMeshProUGUI tutorialText;
    public CanvasGroup notiPanel;

    [Header ("Time wait to hide")]
    public float TimeWait = 2f;
    public void setContent(string content, float time = 0)
    {
        contentText.text = content;
        showNotification(true);
        StartCoroutine(notifyProcess(time));
    }

    public void setTutorial(string txt)
    {
        tutorialText.text = txt;
    }
    IEnumerator notifyProcess(float time)
    {
        if (time == 0) time = TimeWait;
        yield return new WaitForSeconds(time);

        notiPanel.DOFade(0f, 1f).OnComplete ( () => showNotification(false));
    }

    public void showNotification(bool state)
    {
        notiPanel.gameObject.SetActive(state);
        if (state)
        {
            SystemControl.instance.addAction();
        } else
        {
            SystemControl.instance.removeAction();
            setTutorial("");
        }
    }
}
