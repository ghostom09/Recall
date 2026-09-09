using UnityEngine;

public class HurtState : IState
{
    private readonly Enemy _enemy;
    private float _timer;

    public HurtState(Enemy enemy)
    {
        _enemy = enemy;
    }

    public void Enter()
    {
        _enemy.StopMoving();
        _timer = 0.2f;
    }

    public void Update()
    {
        _timer -= Time.fixedDeltaTime;
        if (_timer <= 0f)
        {
            _enemy.StateMachine.ChangeState(_enemy.IdleState);
        }
    }

    public void Exit()
    {

    }
}
