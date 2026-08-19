using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Main Menu Settings")]
    public GraphicRaycaster mainMenuRaycaster;
    public GameObject mainFirstSelected;

    [Header("Settings Menu Settings")]
    public GameObject settingsFirstSelected;

    [Header("Arrow Pointer Settings")]
    public RectTransform arrowPointer;
    public float xOffset = -50f;
    public float moveDuration = 0.2f;

    private GameObject lastSelected;

    void Start()
    {
        OpenMainMenu();
    }

    void Update()
    {
        if (!mainMenuPanel.activeSelf) return;

        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

        if (currentSelected != null && currentSelected != lastSelected)
        {
            AudioSceneController.instance.playArrowChanging();
            MoveArrowTo(currentSelected.GetComponent<RectTransform>());
            lastSelected = currentSelected;
        }
    }

    public void OpenMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);

        if (mainMenuRaycaster != null)
            mainMenuRaycaster.enabled = false;

        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(mainFirstSelected);

        lastSelected = mainFirstSelected;
        if (mainFirstSelected != null)
        {
            SnapArrowTo(mainFirstSelected.GetComponent<RectTransform>());
        }
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);

        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;

        EventSystem.current.SetSelectedGameObject(null);
        if (settingsFirstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(settingsFirstSelected);
        }
    }

    // --- CÁC HÀM XỬ LÝ MŨI TÊN 

    private void MoveArrowTo(RectTransform targetRect)
    {
        if (targetRect == null || arrowPointer == null) return;
        float targetY = targetRect.anchoredPosition.y;
        float targetX = targetRect.anchoredPosition.x + xOffset;

        arrowPointer.DOKill();
        arrowPointer.DOAnchorPosY(targetY, moveDuration).SetEase(Ease.OutBack);
        arrowPointer.DOAnchorPosX(targetX, moveDuration).SetEase(Ease.OutBack);
    }

    private void SnapArrowTo(RectTransform targetRect)
    {
        if (targetRect == null || arrowPointer == null) return;

        arrowPointer.DOKill();
        arrowPointer.anchoredPosition = new Vector2(targetRect.anchoredPosition.x + xOffset, targetRect.anchoredPosition.y);
    }
}