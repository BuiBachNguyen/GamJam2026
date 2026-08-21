using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    
    public virtual void Start()
    {
        player = FindFirstObjectByType<PlayerController>(); 
        LoadData();
    }

    public virtual void LoadData()
    {
        GameObject playerPos = GameObject.Find(ScenePositionController.playerScenePosition);
        GameObject cameraPos = GameObject.Find(ScenePositionController.cameraScenePosition);
        GameObject bounds = GameObject.Find(ScenePositionController.currentCameraBounds);
        Collider2D currentBounds = bounds.GetComponent<Collider2D>();
        player.transform.position = playerPos.transform.position;
        WindowMover.Instance.TeleportToNewRoom(cameraPos.transform.position, currentBounds);
    }

    

}
