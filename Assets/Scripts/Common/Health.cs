using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    public event Action<int, int> HealthChanged;
    public event Action Died;
    public event Action Damaged;
    
    public int Current { get; private set; }
    public int Max { get; private set; }
    public bool IsDead => Current <= 0f;
    public bool IsInvincible => Time.time < _invincibleTime;

    private float _invincibleTime;
    
    public void Initialize(int maxHealth)
    {
        Max = Mathf.Max(maxHealth, 1);
        Current = Max;
        _invincibleTime = 0f;

        HealthChanged?.Invoke(Current, Max);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0f || IsDead || IsInvincible)
            return;

        Current = Mathf.Max(Current - damage, 0);
        Damaged?.Invoke();
        Debug.LogWarning($"<color=blue>{gameObject.name}</color>이 <color=red>{damage}</color>를 입었습니다.");
        HealthChanged?.Invoke(Current, Max);

        if (IsDead)
            Died?.Invoke();
    }

    public void GrantInvincibility(float duration)
    {
        _invincibleTime = Mathf.Max(_invincibleTime, Time.time + Mathf.Max(0f, duration));
    }
}
