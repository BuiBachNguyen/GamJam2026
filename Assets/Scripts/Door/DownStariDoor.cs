using UnityEngine;
using UnityEngine.SceneManagement;

public class DownStariDoor : Door
{
    public override void Move()
    {
        // Door da lo lieu het viec fade man hinh 
        // muc dich ke thua la de di chuyen theo mong muon hoac them am thanh tuy theo loai cua
        ScenePositionController.cameraScenePosition = KeyData.DownStairCamPos;
        ScenePositionController.playerScenePosition = KeyData.DownStairPos;
        ScenePositionController.currentCameraBounds = KeyData.HallBounds;
        //player.Facing = PlayerController.FacingDirection.Down;
        SceneManager.LoadScene(KeyData.LivingroomScene);
    }
}
