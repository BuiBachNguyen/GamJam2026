using UnityEngine;

public class GameParentroomController : GameController
{
    public static GameParentroomController instance;

    void MakeSingleton()
    {
        if (instance == null || instance != this)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleton();
    }
}
