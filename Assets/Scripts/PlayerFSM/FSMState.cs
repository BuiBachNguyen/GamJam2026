using UnityEngine;

public class FSMState
{
    protected FSM fsm;
    protected GameObject obj;
    protected float timer;

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void UpdateState(float delta) { }

    public bool UpdateTimer(float delta)
    {
        if (timer <= 0) return false;
        timer -= delta;
        return timer <= 0;
    }

    public void ChangeState(FSMState newState)
    {
        fsm.ChangeState(newState);
    }

    public void Setup(FSM fsm, GameObject obj)
    {
        this.fsm = fsm;
        this.obj = obj;
    }
}
