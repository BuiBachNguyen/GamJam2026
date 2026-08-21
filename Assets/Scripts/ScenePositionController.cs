using UnityEngine;

/// <summary>
/// This class is used for loading player and camera position in new scene
/// </summary>
public class ScenePositionController : MonoBehaviour
{
    // initial value when enter game

    public static string playerScenePosition = KeyData.PlayerSpawnPoint;

    public static string cameraScenePosition = KeyData.CameraSpawnPoint;

    public static string currentCameraBounds = KeyData.BedroomBounds;
}
