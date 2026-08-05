using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamagable
{
    public event Action<int, int> HealthChanged;
    public event Action Died;
    
    public int Current { get; private set; }
    public int Max { get; private set; }
    public bool IsDead => Current <= 0f;
    
    public void Initialize(int maxHealth)
    {
        Max = Mathf.Max(maxHealth, 1);
        Current = Max;

        HealthChanged?.Invoke(Current, Max);
    }

    public void TakeDamage(int damage)
    {
        if (damage <= 0f || IsDead)
            return;

        Current = Mathf.Max(Current - damage, 0);
        HealthChanged?.Invoke(Current, Max);

        if (IsDead)
            Died?.Invoke();
    }
}
