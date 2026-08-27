using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineUIController : MonoBehaviour
{
    [Header("References")]
    public PlayableDirector director;

    [Tooltip("Kéo Panel UI muốn ẩn vào đây. LƯU Ý: Không gắn script này vào object này!")]
    public GameObject uiPanel;

    public float timeWait;

    public MenuController menuController;

    private void OnEnable()
    {
        if (director != null)
        {
            director.played += OnTimelineStarted;
        }
    }

    private void OnDisable()
    {
        if (director != null)
        {
            director.played -= OnTimelineStarted;
        }
    }

    private void Start()
    {
        if (KeyData.SkipIntro)
        {
            director.gameObject.SetActive(false);
            return;
        }
        // Fix lỗi Play On Awake: Nếu lúc Start mà Timeline đã chạy rồi thì ẩn UI luôn
        if (director != null && director.state == PlayState.Playing)
        {
            OnTimelineStarted(director);
        }
    }

    private void OnTimelineStarted(PlayableDirector obj)
    {
        // Ẩn UI khi Timeline chạy
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            StartCoroutine(process());
        }
    }

    IEnumerator process()
    {
        yield return new WaitForSeconds(timeWait);

        menuController.OpenMainMenu();
    }
}