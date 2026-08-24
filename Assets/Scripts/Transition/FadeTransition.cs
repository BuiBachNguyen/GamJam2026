using DG.Tweening;
using System.Collections;
using UnityEngine;

public class FadeTransition : MonoBehaviour
{

    CanvasGroup canvas;

    public float timeTrans;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        canvas = GetComponent<CanvasGroup>();
        StartCoroutine(beforeLoadSceneFinish());
    }

    public void Fade()
    {
        gameObject.SetActive(true);
        canvas.alpha = 1f;
        canvas.DOFade(0f, timeTrans).OnComplete(() =>
        {
            gameObject.SetActive(false);
            SystemControl.instance.removeAction();
            });
    }

    public void Appear()
    {
        gameObject.SetActive(true);
        canvas.alpha = 0f;
        canvas.DOFade(1f, timeTrans);
    }
    
    IEnumerator beforeLoadSceneFinish()
    {
        yield return new WaitForSeconds(0.5f);

        Fade();

        yield return new WaitForSeconds(1.5f);

        SystemControl.instance.addAction();
    }


}
