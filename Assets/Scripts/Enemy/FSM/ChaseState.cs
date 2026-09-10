using UnityEngine;

public class ChaseState : IState
{
    private readonly Enemy _enemy;

    public ChaseState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {

    }

    public void Update()
    {
        if (_enemy.TryRetreat())
        {
            return;
        }   
        if (_enemy.CanAttack)
        {
            _enemy.StopMoving();
            if (_enemy.AttackReady)
            {
                _enemy.StateMachine.ChangeState(_enemy.AttackState);
            }
            return;
        }

        if (!_enemy.CanChase)
        {
            _enemy.StateMachine.ChangeState(_enemy.IdleState);
            return;
        }

        float deltaX = _enemy.TargetDeltaX;
        if (Mathf.Abs(deltaX) <= Mathf.Max(_enemy.StopDistance, _enemy.speed * Time.fixedDeltaTime))
        {
            _enemy.StopMoving();
            return;
        }
        float direction = Mathf.Sign(deltaX);
        if (!_enemy.CanMove(direction))
        {
            _enemy.StopMoving();
            return;
        }

        _enemy.Move(direction);
    }

    public void Exit()
    {
        _enemy.StopMoving();
    }
}
