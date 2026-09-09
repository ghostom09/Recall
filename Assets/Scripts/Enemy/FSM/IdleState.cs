using UnityEngine;

public class IdleState : IState
{
    private readonly Enemy _enemy;
    private float _waitRemaining;
    

    public IdleState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.StopMoving();
        _waitRemaining = Random.Range(0.5f, 1.5f);
    }

    public void Update()
    {
        if (_enemy.CanChase)
        {
            _enemy.StateMachine.ChangeState(_enemy.ChaseState);
            return;
        }
        _waitRemaining -= Time.fixedDeltaTime;
        if (_waitRemaining <= 0f)
        {
            _enemy.StateMachine.ChangeState(_enemy.MoveState);
        }
    }

    public void Exit()
    {
        
    }
}
