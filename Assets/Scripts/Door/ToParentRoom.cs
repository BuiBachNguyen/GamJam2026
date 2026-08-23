using UnityEngine;
using UnityEngine.SceneManagement;

public class ToParentRoom : Door
{
    public override void Move()
    {
        ScenePositionController.cameraScenePosition = KeyData.InParentRoomCameraSpawn;
        ScenePositionController.playerScenePosition = KeyData.InParentRoomPlayerSpawn;
        ScenePositionController.currentCameraBounds = KeyData.ParentroomBounds;
        SceneManager.LoadScene(KeyData.ParentBedroomScene);
    }
}
