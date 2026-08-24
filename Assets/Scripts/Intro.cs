using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Bắt buộc thêm thư viện này để dùng Image

[System.Serializable]
public class line
{
    public string content;
    public float timeWait;
}

public class Intro : MonoBehaviour
{
    public List<line> lines;
    public TextMeshProUGUI content;

    [Header("Image Effect")]
    public Image introImage;      // Gắn UI Image vào đây
    public int indexToShow = 1;   // Index câu thoại muốn hiện ảnh
    public float maxScale = 1.2f; // Kích thước phóng to tối đa (1.2 = to 120%)

    int index = 0;

    private void OnEnable()
    {
        index = 0;

        if (content != null)
        {
            content.color = new Color(content.color.r, content.color.g, content.color.b, 0f);
        }

        // Đảm bảo tấm ảnh bị thu nhỏ hết cỡ và tắt đi khi mới bắt đầu
        if (introImage != null)
        {
            introImage.transform.localScale = Vector3.zero;
            introImage.gameObject.SetActive(false);
        }

        AudioManager.Instance.PlayBGM(1);
    }

    public void play()
    {
        StopAllCoroutines();
        index = 0;
        StartCoroutine(process());
    }

    IEnumerator process()
    {
        while (index < lines.Count)
        {
            // 1. Set nội dung
            content.text = lines[index].content;

            // 2. Fade In chữ
            content.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.5f);

            // ==========================================
            // LOGIC SHOW HÌNH ẢNH
            // ==========================================
            if (index == indexToShow && introImage != null)
            {
                introImage.gameObject.SetActive(true);

                // Chia đôi thời gian chờ của câu thoại hiện tại
                float halfTime = 4f;

                Sequence imgSeq = DOTween.Sequence();

                // Nửa đầu: Phóng to từ 0 lên maxScale
                imgSeq.Append(introImage.transform.DOScale(maxScale, halfTime).SetEase(Ease.OutQuad));

                imgSeq.AppendInterval(2f);

                // Nửa sau: Thu nhỏ dần về 0
                imgSeq.Append(introImage.transform.DOScale(0f, halfTime).SetEase(Ease.InQuad));
            }
            // ==========================================

            // 3. Chờ thời gian hiển thị của câu thoại
            // (Trong lúc chờ này, cái Sequence của ảnh ở trên sẽ tự động chạy song song)
            yield return new WaitForSeconds(lines[index].timeWait);

            // 4. Fade Out chữ
            content.DOFade(0f, 0.5f);
            yield return new WaitForSeconds(0.5f);

            index++;
        }

        Debug.Log("Đã chạy xong Intro!");

        SceneManager.LoadScene(KeyData.MainBedroomScene);
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBGM();
            AudioManager.Instance.PlayBGM("BackgroundSound");
            AudioManager.Instance.PlaySFX(AudioClipNames.UIButton);
        }
    }


}