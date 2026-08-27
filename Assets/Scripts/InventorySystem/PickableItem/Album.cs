
using UnityEngine;

public class Album : PickableItem
{
    public Sprite albumOpen;
    public Sprite albumClose;
    SpriteRenderer sp;

    public GameObject panel;

    private void OnEnable()
    {
        EventController.canInteractWithAlbum += onFinishAlbum;
    }

    private void OnDisable()
    {
        EventController.canInteractWithAlbum -= onFinishAlbum;
    }

    void onFinishAlbum(bool state)
    {
        canShowTuto = state;
    }

    public override void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        // luu trang thai dong mo cuon so
        if (PlayerPrefs.GetInt("Album") == 0) // close
        {
            sp.sprite = albumClose;
        }
        else if (PlayerPrefs.GetInt("Album") == 1)
        {
            sp.sprite = albumOpen;
        }
        if (PlayerPrefs.GetInt("Album") == 2 )
        {
            // khong cho tuong tac
            GetComponent<Collider2D>().enabled = false;
        } else
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    public override void PickUpProcess(Collider2D collision)
    {
        
        if (!player.IsInteract) return;
        player.IsInteract = false;
        if (PlayerPrefs.GetInt("Album") == 2)
        {
            // khong cho tuong tac
            GetComponent<Collider2D>().enabled = false;
            return;
        }
        TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
        sp.sprite = albumOpen;
        // thuc hien ham gi do o day
        if (panel != null) //open nha
        {
            SystemControl.instance.addAction();
            panel.SetActive(true);
        }

    }
    private void Update()
    {
        if (panel != null)
        {
            if(Input.GetKeyDown(KeyCode.Escape) && panel.activeSelf)
            {
                //PlayerPrefs.SetInt("Album", 0);
                sp.sprite = albumClose;
                panel.SetActive(false);
                SystemControl.instance.removeAction();
            }
        } 
    }
}
