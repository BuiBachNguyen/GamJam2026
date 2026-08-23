using UnityEngine;
using UnityEngine.SceneManagement;

public class ToParentRoom : Door
{
    public bool PuzzleIsSolve = false;
    public GameObject Puzzle; 
    public override void Move()
    {
        ScenePositionController.cameraScenePosition = KeyData.InParentRoomCameraSpawn;
        ScenePositionController.playerScenePosition = KeyData.InParentRoomPlayerSpawn;
        ScenePositionController.currentCameraBounds = KeyData.ParentroomBounds;
        SceneManager.LoadScene(KeyData.ParentBedroomScene);
    }

    public override void OnInteract()
    {
        if(PuzzleIsSolve)
        {
            base.OnInteract();
        }
        else
        {
            Puzzle.SetActive(true);
        }    
        
    }
    public void OnSolved()
    {
        this.PuzzleIsSolve = true;
        Puzzle.SetActive(false);
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
