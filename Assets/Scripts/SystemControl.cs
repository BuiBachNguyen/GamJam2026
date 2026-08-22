using UnityEngine;

public class SystemControl : MonoBehaviour
{
    private int currentAction;
    public bool forceAllowSwitchMode = false;

    public static SystemControl instance;

    void MakeSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        MakeSingleton();
    }

    private void Start()
    {
        currentAction = 1;
        forceAllowSwitchMode = false;
    }

    public bool freezeKeyboard()
    {
        return currentAction > 0;
    }

    public void addAction()
    {
        currentAction++;
    }

    public void removeAction()
    {
        currentAction--;
    }
}
