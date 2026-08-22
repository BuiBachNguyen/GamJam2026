using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    void MakeSingleton()
    {
        if (instance == null || instance != this)
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

    public GameObject InteractionButton;
    public GameObject SwitchModeButton;
    public Vector3 offset = new Vector3(0, 1.3f, 0);
    public void ShowTutorialInteraction(bool state, Vector3 postion)
    {
        InteractionButton.SetActive(state);
        if (state)
        {
            InteractionButton.transform.position = postion + offset;
        }
    }

    public void ShowTutorialSwitchMode(bool state, Vector3 position)
    {
        SwitchModeButton.SetActive(state);
        if (state)
        {
            SwitchModeButton.transform.position = position + offset;
        }
    }

}
