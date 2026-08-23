using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class KeyData : MonoBehaviour
{
    [Header("Scene name")]
    public static string MainBedroomScene = "MainBedRoomScene";
    public static string LivingroomScene = "LivingRoomScene";
    public static string KitchenScene = "KitchenScene";
    public static string ParentBedroomScene = "ParentBedroomScene";

    [Header("Position spawn")]
    public static string DownStairPos = "DownStairPos";
    public static string DownStairCamPos = "DownStairCamPos";
    public static string HallBounds = "HallBounds";
    public static string LivingroomBounds = "LivingroomBounds";
    public static string PlayerSpawnPoint = "PlayerSpawnPoint";
    public static string CameraSpawnPoint = "CameraSpawnPoint";
    public static string BedroomBounds = "BedroomBounds";
    public static string UpStairPos = "UpStairPos";
    public static string UpStairCamPos = "UpStairCamPos";
    public static string InParentRoomPlayerSpawn = "PlayerSpawn";
    public static string InParentRoomCameraSpawn = "CameraSpawn";
    public static string ParentroomBounds = "ParentroomBounds";

    [Header("ID object")]
    public static int KeyLivingroom = 1;
    public static int KeyKitchen = 4;

    [Header("Saved")]
    public static string StartConversation = "StartConversation";
}
