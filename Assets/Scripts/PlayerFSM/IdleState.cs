using UnityEngine;

public class IdleState : FSMState
{
    PlayerController player;
    Rigidbody2D rb;

    public override void Enter()
    {
        player = obj.GetComponent<PlayerController>();
        rb = obj.GetComponent<Rigidbody2D>();
        Debug.Log("IDLE enter");
    }

    public override void UpdateState(float delta)
    {
        player.HandleAnimated(this);
        player.HandleMoving();
    }
}
