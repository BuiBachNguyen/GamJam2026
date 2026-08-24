using TMPro;
using UnityEngine;

public class PCUIController : MonoBehaviour
{
    public static PCUIController instance;

    void MakeSingleton()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }

    }

    private void Awake()
    {
        MakeSingleton();
    }

    [Header("UI")]
    public GameObject PCPanel;
    public GameObject OffScreenPanel;
    public GameObject PassPanel;
    public TextMeshProUGUI notifyText;
    public GameObject OpenFolderPanel;


    public void setNotifyText(string txt)
    {
        notifyText.gameObject.SetActive(true);  
        notifyText.text = txt;
    }

    public void showPCPanel(bool state)
    {
        // Nếu trạng thái của panel đã đúng như mong muốn rồi thì dừng luôn, không làm gì cả
        if (PCPanel.activeSelf == state) return;

        PCPanel.SetActive(state);
        if (state)
        {
            CloseOffScreen();
            ClosePassPanel();
            SystemControl.instance.addAction();
        }
        else
        {
            SystemControl.instance.removeAction();
        }
    }

    public void showOffScreenPanel(bool state)
    {
        OffScreenPanel.SetActive(state);
    }

    public void showPassPanel(bool state)
    {
        PassPanel.SetActive(state);
    }

    public void OpenOffScreen()
    {
        showOffScreenPanel(true);
    }

    public void CloseOffScreen()
    {
        showOffScreenPanel(false);
    }

    public void OpenPassPanel()
    {
        showPassPanel(true);
    }

    public void ClosePassPanel()
    {
        showPassPanel(false);
    }

    public void clickOffScreenBtn()
    {
        if (OffScreenPanel.activeSelf)
        {
            CloseOffScreen();
        } else
        {
            OpenOffScreen();
        }
    }

    public void showOpenFolder(bool state)
    {
        OpenFolderPanel.SetActive(state);
        if (state)
        {
            OpenFolderPanel.GetComponent<CanvasGroup>().alpha = 0f;
        } else
        {
            OpenFolderPanel.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }
}
