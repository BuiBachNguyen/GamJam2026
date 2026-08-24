using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private InputAction moveAction;

    private void Awake()
    {
        moveAction = playerInput.actions.FindAction("Move");
    }

    private void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        //Debug.Log(move);
        this.transform.position += new Vector3(move.x, move.y, 0f);
    }
}