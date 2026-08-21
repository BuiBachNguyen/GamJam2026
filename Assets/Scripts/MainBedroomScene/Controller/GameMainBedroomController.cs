using UnityEngine;

public class GameMainBedroomController : GameController
{
    public Collider2D hallBounds, bedroomBounds;

    public static GameMainBedroomController instance;

    void MakeSingleton()
    {
        if (instance == null)
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

    public Collider2D getBounds(int id)
    {
        return id == 1 ?bedroomBounds : hallBounds;
    }

}
