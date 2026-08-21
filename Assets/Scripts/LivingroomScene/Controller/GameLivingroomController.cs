using UnityEngine;

public class GameLivingroomController : GameController
{
    public Collider2D hallBounds, livingroomBounds;

    public static GameLivingroomController instance;

    void MakeSingleton()
    {
        if (instance == null)
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

    public Collider2D getBounds(int id)
    {
        return id == 1 ? hallBounds : livingroomBounds;
    }
}
