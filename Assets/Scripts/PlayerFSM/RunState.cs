using UnityEngine;

public class RunState : FSMState
{
    PlayerController player;
    Rigidbody2D rb;

    public override void Enter()
    {
        player = obj.GetComponent<PlayerController>();
        rb = obj.GetComponent<Rigidbody2D>();
    }

    public override void UpdateState(float delta)
    {
        player.UpdateFacing(rb.linearVelocity);
        player.HandleAnimated(this);
        player.HandleMoving();

        if (player.HandleMoving() == false)
        {
            ChangeState(new IdleState());
        }
    }
}
