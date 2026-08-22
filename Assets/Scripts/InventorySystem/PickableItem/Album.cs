using UnityEngine;

public class Album : PickableItem
{
    public Sprite albumOpen;
    public Sprite albumClose;
    SpriteRenderer sp;

    

    public override void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        // luu trang thai dong mo cuon so
        if (PlayerPrefs.GetInt("Album") == 0) // close
        {
            sp.sprite = albumClose;
        }
        else if (PlayerPrefs.GetInt("Album")  == 1)
        {
             sp.sprite = albumOpen;
        }
    }
    public override void PickUpProcess(Collider2D collision)
    {
        if (!player.IsInteract) return;
        if (sp.sprite != albumClose) return; // dang mo thi khong cho tuong tac
        GetComponent<Collider2D>().enabled = false;
        sp.sprite = albumOpen;
        // thuc hien ham gi do o day
    }
}
