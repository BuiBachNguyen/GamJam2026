using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class DialogController : MonoBehaviour
{
    public static DialogController instance;

    void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
        }
    }

    private void Awake()
    {
        MakeSingleton();
    }

    int index = 0;
    Dialog currentDialog;

    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private Action onCompleteDialog = null;

    [Header("UI")]
    public GameObject DialogPanel;
    public TextMeshProUGUI charNameDialog;
    public Image charAvaDialog;
    public TextMeshProUGUI dialogContent;

    public GameObject nextButton;

    private void Start()
    {
        InitData();
    }

    void InitData()
    {
        index = 0;
        isTyping = false;
        onCompleteDialog = null;
    }

    public void playDialog(Dialog dialogInfo, Action action = null)
    {
        index = 0;
        showDialogPanel(true);
        setDialogInfo(dialogInfo.CharName, dialogInfo.CharAvatar);
        currentDialog = dialogInfo;
        onCompleteDialog = action;
        StartDialog(); 
    }

    public void showDialogPanel(bool state)
    {
        // Thêm dòng chặn này tương tự
        if (DialogPanel.activeSelf == state) return;

        DialogPanel.SetActive(state);
        if (state)
        {
            FocusOntarget(nextButton);
            SystemControl.instance.addAction();
        }
        else
        {
            SystemControl.instance.removeAction();
        }
    }

    public void FocusOntarget(GameObject gameObject)
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void setDialogInfo(string dialogName, Sprite dialogAva)
    {
        this.charAvaDialog.gameObject.SetActive(true);
        charNameDialog.text = dialogName;
        if (dialogAva != null)
        {
            charAvaDialog.sprite = dialogAva;
        } else
        {
            charAvaDialog.gameObject.SetActive(false);
        }
    }

    // ham de skip
    public void OnInteractDialog()
    {
        if (isTyping)
        {
            CompleteLine();
        }
        else
        {
            NextLine();
        }
    }

    public void StartDialog()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        typingCoroutine = StartCoroutine(TypeLine());
    }

    public void NextLine()
    {
        index++;
        if (index < currentDialog.lines.Count)
        {
            StartDialog();
        }
        else
        {
            showDialogPanel(false);
            if (onCompleteDialog != null)
            {
                onCompleteDialog.Invoke();
                onCompleteDialog = null;
            }
        }
    }

    private void CompleteLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        dialogContent.text = currentDialog.lines[index];
        isTyping = false;
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogContent.text = ""; 

        foreach (char c in currentDialog.lines[index])
        {
            dialogContent.text += c;
            yield return new WaitForSeconds(currentDialog.typeSpeed);
        }

        isTyping = false; 
    }
}