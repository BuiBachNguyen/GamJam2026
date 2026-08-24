using UnityEngine;

public class FSM : MonoBehaviour
{
    public FSMState currentState;
    public GameObject owner;

    private void Awake()
    {
        owner = this.gameObject;
    }

    void FixedUpdate()
    {
        currentState?.UpdateState(Time.deltaTime);
    }

    public void ChangeState(FSMState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        if (currentState != null)
        {
            currentState.Setup(this, owner);
            currentState.Enter();
        }
    }
}
