using System;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static event Action useRemoteFirstTime;

    public static Action<bool> canInteractWithLocker;

    public static Action<bool> canInteractWithAlbum;
}
