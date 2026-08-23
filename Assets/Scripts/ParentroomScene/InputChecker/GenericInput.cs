using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GenericInput : MonoBehaviour
{
    [Header("Settings")]
    public int maxDigits = 4;

    [Header("Pointer Settings")]
    public RectTransform pointerArrow; // Kéo UI Image mũi tên vào đây
    public float yOffset = 70f; // Khoảng cách từ mũi tên đến tâm ô số (chỉnh tùy ý)

    [Header("UI References")]
    public Transform slotContainer;
    public GameObject digitSlotPrefab; // Chú ý: Prefab này giờ phải có script DigitSlot

    [Header("Events")]
    public UnityEvent<string> OnCodeCompleted;

    private List<DigitSlot> slots = new List<DigitSlot>();
    private string currentInput = "";

    private void Start()
    {
        StartCoroutine(init());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            string inputStr = Input.inputString;

            if (!string.IsNullOrEmpty(inputStr))
            {
                char c = inputStr[0];

                // Nếu phím bấm là số (0-9)
                if (char.IsDigit(c))
                {
                    // Chuyển ký tự thành số nguyên và gọi hàm nhập
                    EnterDigit(int.Parse(c.ToString()));
                }
                // Nếu phím bấm là nút xóa (Backspace)
                else if (c == '\b')
                {
                    RemoveLastDigit();
                }
            }
        }
    }

    IEnumerator init()
    {
        InitializeSlots();

        yield return null;

        UpdateUI();
    }    

    private void InitializeSlots()
    {
        for (int i = 0; i < maxDigits; i++)
        {
            GameObject slotObj = Instantiate(digitSlotPrefab, slotContainer);
            DigitSlot slotUI = slotObj.GetComponent<DigitSlot>();
            slots.Add(slotUI);
        }
    }

    public void EnterDigit(int digit)
    {
        if (currentInput.Length < maxDigits)
        {
            currentInput += digit.ToString();
            UpdateUI();

            if (currentInput.Length == maxDigits)
            {
                OnCodeCompleted?.Invoke(currentInput);
            }
        }
    }

    public void RemoveLastDigit()
    {
        if (currentInput.Length > 0)
        {
            currentInput = currentInput.Substring(0, currentInput.Length - 1);
            UpdateUI();
        }
    }

    public void ClearInput()
    {
        currentInput = "";
        UpdateUI();
    }
    private void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < currentInput.Length)
                slots[i].SetFilled(currentInput[i]);
            else if (i == currentInput.Length)
                slots[i].SetActive();
            else
                slots[i].SetEmpty();
        }
        UpdatePointerPosition(currentInput.Length);
    }

    private void UpdatePointerPosition(int activeIndex)
    {
        if (pointerArrow == null) return;
        if (activeIndex >= maxDigits)
        {
            pointerArrow.gameObject.SetActive(false);
            return;
        }
        pointerArrow.gameObject.SetActive(true);
        Vector2 targetPos = slots[activeIndex].GetComponent<RectTransform>().anchoredPosition;
        targetPos.y += yOffset; 

        pointerArrow.anchoredPosition = targetPos;

    }

}
