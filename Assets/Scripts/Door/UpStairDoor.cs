using UnityEngine;
using UnityEngine.SceneManagement;

public class UpStairDoor : Door
{
    public override void Move()
    {
        ScenePositionController.cameraScenePosition = KeyData.UpStairCamPos;
        ScenePositionController.playerScenePosition = KeyData.UpStairPos;
        ScenePositionController.currentCameraBounds = KeyData.HallBounds;
        //player.Facing = PlayerController.FacingDirection.Down;
        SceneManager.LoadScene(KeyData.MainBedroomScene);
    }
}
