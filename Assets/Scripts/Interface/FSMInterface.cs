using UnityEngine;

public interface IAttack
{
    public void Attack();
}

public interface IMovement
{
    public void Move(float direction);
}

public interface IChase
{
    public void Chase();
}

public interface IHurt
{
    public void Hurt();
}

public interface IState
{
    public void Enter();
    public void Exit();
    public void Update();
}
