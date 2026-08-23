using UnityEngine;

public class PickUpState : FSMState
{
    PlayerController player;
    Rigidbody2D rb;

    public override void Enter()
    {
        player = obj.GetComponent<PlayerController>();
        rb = obj.GetComponent<Rigidbody2D>();
        player.CanMove = false;
        rb.linearVelocity = Vector2.zero; // dừng player lại

        player.HandleAnimated(this);
        timer = player.Animator.GetCurrentAnimatorStateInfo(0).length;
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioClipNames.PickUp);
    }

    public override void UpdateState(float delta)
    {
        player.HandleAnimated(this);
        if (UpdateTimer(delta))
        {
            ChangeState(new IdleState());
            player.CanMove = true;
            player.IsInteract = false;
        }
    }
}
