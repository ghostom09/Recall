using UnityEngine;

public class MoveState : IState
{
    private readonly Enemy _enemy;
    private float _direction;
    private float _timeRemaining;


    public MoveState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _direction = 1f;
        if (Random.Range(0, 2) == 0)
        {
            _direction = -1f;
        }
        _timeRemaining = 1f;
    }

    public void Update()
    {
        if (_enemy.CanChase)
        {
            _enemy.StateMachine.ChangeState(_enemy.ChaseState);
            return;
        }

        _timeRemaining -= Time.fixedDeltaTime;
        if (_timeRemaining <= 0f || !_enemy.CanMove(_direction))
        {
            _enemy.StateMachine.ChangeState(_enemy.IdleState);
            return;
        }

        _enemy.Move(_direction);
    }


    public void Exit()
    {
        _enemy.StopMoving();
    }
}
