using UnityEngine;

public class AttackState : IState
{
    private readonly Enemy _enemy;
    private float _timer;
    private float _direction;

    public AttackState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.StopMoving();
        _direction = Mathf.Sign(_enemy.TargetDeltaX);
        Debug.Log("적이 공격중입니다");
        _timer = 0.3f;
        _enemy.StartAttackCooldown();
    }

    public void Update()
    {
        _timer -= Time.fixedDeltaTime;
        if (_timer > 0f) return;

        _enemy.Attack(_direction);
        Debug.Log("적이 공격했습니다");
        _enemy.StateMachine.ChangeState(_enemy.IdleState);
    }

    public void Exit()
    {

    }
}
