using UnityEngine;

public class AudioSceneController : MonoBehaviour
{
    public static AudioSceneController instance;

    void MakeSingleton()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleton();
    }

    #region SoundSource
    [Header("AudioSource")]
    public AudioSource effectSource;

    [Header("SoundEffect")]
    public AudioClip arrowChangingSound;


    #endregion

    #region Function
    public void playArrowChanging()
    {
        effectSource.PlayOneShot(arrowChangingSound);
    }
    #endregion
}
