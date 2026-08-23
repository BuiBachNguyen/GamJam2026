using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
public class Door : MonoBehaviour
{
    


    // ===== MOVE TYPE =====
    // Move in one scene
    // Move from one scene to another
    public GameObject newPlayerPosition;
    public GameObject newCameraPosition;
    public Collider2D newBounds;
    public GameObject FadeCanvas;
    // if not then we will show a tutorial

    private Collider2D collider;
    protected PlayerController player;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }


    private void Start()
    {

    }


    /// <summary>
    /// On Default, this function will be used between places in a single scene 
    /// Different Scenes loading requires overriding
    /// </summary>
    public virtual void Move()
    {
        if (player !=  null)
        {
            player.transform.position = newPlayerPosition.transform.position; // move player to a new Place
            if(AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySFX(AudioClipNames.OpenDoor);
            }    
            WindowMover.Instance.TeleportToNewRoom(newCameraPosition.transform.position, newBounds);  // move Camera and Set new collider bounds
            StartCoroutine(fadeProcess());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TutorialManager.instance.ShowTutorialInteraction(true, collision.gameObject.transform.position);
            this.player = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
            this.player = null;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.IsInteract)
            {
                player.IsInteract = false;
                OnInteract();
            }
        }
    }

    public virtual void OnInteract ()
    {
        SystemControl.instance.addAction();
        TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero); // close tutorial
        // Fade Camera
        FadeTransition transition = FadeCanvas.GetComponent<FadeTransition>();
        if (transition != null)
        {
            StartCoroutine(playTransition(transition, TypeTransition.Appear));
        }
    }

    // need to forbid player movement and player keyboard using
    IEnumerator playTransition(FadeTransition trans, TypeTransition type)
    {
        if (type == TypeTransition.Appear)
        {
            trans.Appear();
        } else
        {
            trans.Fade();
        }
        yield return new WaitForSeconds(trans.timeTrans + 1f);

        Move();
    }

    IEnumerator fadeProcess()
    {
        yield return new WaitForSeconds(1);
        FadeTransition trans = FadeCanvas.GetComponent<FadeTransition>();
        trans.Fade(); // remove action nam o day
        yield return new WaitForSeconds(trans.timeTrans + 1f);
    }
    

    enum TypeTransition { Appear, Fade};



}
