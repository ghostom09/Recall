using UnityEngine;

public class EnemyStateMachine
{
    public IState CurrentState { get; private set; }

    public void ChangeState(IState nextState)
    {
        if (nextState == null || nextState == CurrentState)
            return;

        CurrentState?.Exit();
        CurrentState = nextState;
        CurrentState.Enter();
    }

    public void Update()
    {
        CurrentState?.Update();
    }
}