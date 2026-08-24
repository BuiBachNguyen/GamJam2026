using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHall : Door
{
    public override void Move()
    {
        ScenePositionController.cameraScenePosition = KeyData.CamInFrontParent;
        ScenePositionController.playerScenePosition = KeyData.InFrontParent;
        ScenePositionController.currentCameraBounds = KeyData.HallBounds;
        SceneManager.LoadScene(KeyData.MainBedroomScene);
    }
}
