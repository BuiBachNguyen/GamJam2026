using UnityEngine;
using UnityEngine.SceneManagement;

public class ToParentRoom : Door
{
    public bool PuzzleIsSolve = false;
    public GameObject Puzzle;
    public Dialog dialog;
    public override void Move()
    {
        ScenePositionController.cameraScenePosition = KeyData.InParentRoomCameraSpawn;
        ScenePositionController.playerScenePosition = KeyData.InParentRoomPlayerSpawn;
        ScenePositionController.currentCameraBounds = KeyData.ParentroomBounds;
        SceneManager.LoadScene(KeyData.ParentBedroomScene);
    }

    public override void OnInteract()
    {
        if (PlayerPrefs.GetInt("Puzzle") == 1)
        {
            base.OnInteract();
        } else
        {
            TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
            DialogController.instance.playDialog(dialog, () =>
            {
                Puzzle.SetActive(true);
                SystemControl.instance.addAction();

            });
        }
        //if (PuzzleIsSolve)
        //{
        //    base.OnInteract();
        //}
        //else
        //{
        //    TutorialManager.instance.ShowTutorialInteraction(false, Vector3.zero);
        //    DialogController.instance.playDialog(dialog, () =>
        //    {
        //        Puzzle.SetActive(true);
        //        SystemControl.instance.addAction();

        //    });

        //}    
        
    }
    public void OnSolved()
    {
        this.PuzzleIsSolve = true;
        Puzzle.SetActive(false);
        SystemControl.instance.removeAction();
        PlayerPrefs.SetInt("Puzzle", 1);
        base.OnInteract();
    }    
    private void Awake()
    {
        GameSlidePuzzle.IsSolved -= OnSolved;
        GameSlidePuzzle.IsSolved += OnSolved;
        Puzzle.SetActive(false);

    }
    private void OnDestroy()
    {
        GameSlidePuzzle.IsSolved -= OnSolved;
    }
}
