using System.Collections;
using UnityEngine;

public class GameMainBedroomController : GameController
{
    public Collider2D hallBounds, bedroomBounds;

    public static GameMainBedroomController instance;

    public FadeTransition fadeTransition;
    public Dialog startParentDialog, mainCharStartDialog;

    void MakeSingleton()
    {
        if (instance == null)
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

    public override void Start()
    {
        base.Start();
        if (PlayerPrefs.GetInt(KeyData.StartConversation) == 1) return;
        StartCoroutine(firstConversation());
    } 

    public Collider2D getBounds(int id)
    {
        return id == 1 ?bedroomBounds : hallBounds;
    }

    IEnumerator firstConversation()
    {
        yield return new WaitForSeconds(1f + fadeTransition.timeTrans);

        DialogController.instance.playDialog(startParentDialog, () =>
        {
            StartCoroutine(DelayNextDialog(mainCharStartDialog));
        });
    }

    IEnumerator DelayNextDialog(Dialog nextDialog)
    {
        yield return null; // khuc nay nen doi tieng buoc chan xuong cau thanh roi goi
        DialogController.instance.playDialog(nextDialog);
        PlayerPrefs.SetInt(KeyData.StartConversation, 1);
    }

}
