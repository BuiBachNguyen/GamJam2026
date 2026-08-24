using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    Button btn;
    public GameObject introPanel;
    public GameObject menuPanel;

    private void Awake()
    {
        btn = GetComponent<Button>();
    }

    private void Start()
    {
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(Enter);
        }
    }

    void Enter()
    {
        introPanel.SetActive(true);
        introPanel.GetComponent<CanvasGroup>().alpha = 0f;
        introPanel.GetComponent<CanvasGroup>().DOFade(1f, 1f).OnComplete(() =>
        {
            menuPanel.SetActive(false);
            introPanel.GetComponent<Intro>().play();
        });
        
    }


}
