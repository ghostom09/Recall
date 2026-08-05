using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnemyStat data;
    [SerializeField] private Health health;

    private void Awake()
    {
        health.Initialize(data.MaxHealth);
    }

    private void OnEnable()
    {
        health.Died += HandleDeath;
    }

    private void OnDisable()
    {
        health.Died -= HandleDeath;
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }
}
