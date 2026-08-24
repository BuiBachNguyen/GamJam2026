using UnityEngine;
using TMPro;

public class DigitSlot : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI numberText;

    // 1. Trạng thái trống
    public void SetEmpty()
    {
        numberText.text = "";
    }

    // 2. Trạng thái Active (đang chờ nhập)
    public void SetActive()
    {
        numberText.text = "_"; // Hiện dấu gạch dưới để biết đang ở ô này
    }

    // 3. Trạng thái đã điền số
    public void SetFilled(char digit)
    {
        numberText.text = digit.ToString();
    }
}