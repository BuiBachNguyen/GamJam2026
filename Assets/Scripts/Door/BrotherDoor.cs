using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BrotherDoor : Door
{
    public string notifyString = "[Ở đây có một lỗ nhỏ, có thể tra thứ gì đó vào được.]";
    public string afterString = "[Đã sử dụng tay nắm cửa.]";
    public float timeWait = 2f;
    public Dialog dialog;
    public Dialog AfterDialog;
    public Dialog FinalDialog;
    public Dialog ParentDialog;
    public GameObject blurCanvas;
    public GameObject picture;
    public GameObject TheEndPanel;
    public GameObject IngamePanel;


    bool havePress;
    private void Start()
    {
        havePress = false;
    }

    private void Update()
    {
        if (Input.anyKeyDown && !havePress && TheEndPanel.activeSelf)
        {
            havePress = true;
            KeyData.SkipIntro = true;
            SceneManager.LoadScene(KeyData.MenuScene);
        }
    }



    public override void OnInteract()
    {
        TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
        if (InventorySystem.instance.getInventory(KeyData.DoorNumb) != null)
        {
            NotificationUIController.instance.setContent(afterString, timeWait);

            StartCoroutine(process());
        } else
        {
            NotificationUIController.instance.setContent(notifyString, timeWait);
            StartCoroutine(conversation());
        }
    }

    IEnumerator process()
    {
        yield return new WaitForSeconds(timeWait + 0.5f);

        AudioManager.Instance.StopBGM();

        AudioManager.Instance.PlayPlayerSFX(AudioClipNames.HeartBead);

        IngamePanel.SetActive(false);

        base.OnInteract();

        for (int i=0;i<2;i++)
        {
            SystemControl.instance.addAction();
        }

        yield return new WaitForSeconds(2f);


        AudioManager.Instance.PlayBGM(2);

        blurCanvas.SetActive(true);

        DialogController.instance.playDialog(AfterDialog, () =>
        {
            picture.SetActive(true);
            picture.GetComponent<CanvasGroup>().alpha = 0f;
            picture.GetComponent<CanvasGroup>().DOFade(1f, 1f).OnComplete(() =>
            {
                DialogController.instance.playDialog(FinalDialog, () =>
                {
                    
                    StartCoroutine(parentConversation());
                });
            });
        });
    }

    IEnumerator parentConversation()
    {

        AudioManager.Instance.StopBGM();

        AudioManager.Instance.PlayPlayerSFX(AudioClipNames.LongWalk);

        Coroutine shakeRoutine = StartCoroutine(FootstepShakeRoutine());

        yield return new WaitForSeconds(2f); 

        AudioManager.Instance.PlaySFX(AudioClipNames.LastSound);

        yield return new WaitForSeconds(2.5f);

        AudioManager.Instance.stopPlayerSFX();

        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        DialogController.instance.playDialog(ParentDialog);

        DialogController.instance.showNextButton(false);

        yield return new WaitForSeconds(1f);

        DialogController.instance.NextLine();

        Debug.Log("Is running last sound");

        TheEndPanel.SetActive(true);
    }

    IEnumerator FootstepShakeRoutine()
    {
        float timeBetweenSteps = 0.4f;
        float shakeDuration = 0.3f;

        // Rung theo trục X (trái/phải) và Y (lên/xuống)
        // Ví dụ: rung trục Y mạnh hơn trục X một chút để mô phỏng bước đi tự nhiên hơn
        float shakeX = 0.8f;
        float shakeY = 1.2f;
        Vector3 shakeStrength = new Vector3(shakeX, shakeY, 0);


        while (true)
        {
            // Dừng nhịp rung cũ (nếu có) trước khi rung nhịp mới để camera không bị lệch
            picture.transform.DOComplete();

            // Thực hiện rung
            picture.transform.DOShakePosition(shakeDuration, shakeStrength, vibrato: 1, randomness: 0, fadeOut: true);

            // Đợi một khoảng thời gian bằng 1 nhịp bước chân rồi mới lặp lại
            yield return new WaitForSeconds(timeBetweenSteps);
        }
    }


    IEnumerator conversation()
    {
        yield return new WaitForSeconds(timeWait + 0.5f);

        DialogController.instance.playDialog(dialog);
    }
}
