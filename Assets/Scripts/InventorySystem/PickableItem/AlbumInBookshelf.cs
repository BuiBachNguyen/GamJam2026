using System.Collections;
using UnityEngine;

public class AlbumInBookshelf : PickableItem
{
    public Dialog dialog;
    public FadeTransition fadeTransition;
    public GameObject playerPos;
    public GameObject cameraPos;
    public GameObject album;
    public override void PickUpProcess(Collider2D collision)
    {
        if (havePicked) return;
        if (!player.IsInteract) return;
        havePicked = true;
        player.IsInteract = false;
        GetComponent<Collider2D>().enabled = false;
        DialogController.instance.playDialog(dialog, () =>
        {
            PlayerPrefs.SetInt("Object" + id, 1);
            GameLivingroomController.instance.StartCoroutine(fade());
            Destroy(gameObject);
        });
    }

    IEnumerator fade()
    {
        fadeTransition.Appear();

        yield return new WaitForSeconds(fadeTransition.timeTrans + 0.5f);

        player.transform.position = playerPos.transform.position;
        player.Facing = PlayerController.FacingDirection.Down;
        player.HandleAnimated(new IdleState());
        Camera.main.transform.position = cameraPos.transform.position;
        album.SetActive(true);

        fadeTransition.Fade();
    }
}
