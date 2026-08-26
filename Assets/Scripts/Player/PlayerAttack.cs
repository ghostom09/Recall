using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform attackPivot;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new(2f, 1f);
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float attackSpeed;
    [SerializeField] private GameObject attackEffect;

    private bool _canAttack = true;
    private bool _isFacingRight = true;
    private int _damage;
    private GameObject _tempAttackEffect;
    private float _attackTimer;

    private void Update()
    {
        UpdateTimer();
    }

    public void GetAttackDamage(int damage)
    {
        _damage = damage;
    }

    public bool TryAttack(out bool hitEnemy)
    {
        if (!_canAttack)
        {
            hitEnemy = false;
            return false;
        }
        
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Vector2 direction =
            (mouseWorldPosition - (Vector2)transform.position).normalized;
        
        UpdateAttackDirection(direction.x);
        hitEnemy = ApplyDamage();
        return true;
    }

    private void UpdateAttackDirection(float direction)
    {
        if (direction > 0f)
        {
            _isFacingRight = true;
        }
        else if (direction < 0f)
        {
            _isFacingRight = false;
        }

        float angle = _isFacingRight ? 0f : 180f;

        attackPivot.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    private bool ApplyDamage()
    {
        
        _tempAttackEffect = Instantiate(attackEffect, attackPoint.position, Quaternion.identity);
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            attackPoint.position,
            attackSize,
            attackPoint.eulerAngles.z,
            enemyLayer
        );

        HashSet<IDamageable> damagedTargets = new();

        foreach (Collider2D hit in hits)
        {
            IDamageable target = hit.GetComponentInParent<IDamageable>();

            if (target == null || !damagedTargets.Add(target))
                continue;

            target.TakeDamage(_damage);
        }

        Destroy(_tempAttackEffect, 1f);
        _canAttack = false;
        return damagedTargets.Count > 0;
    }

    private void UpdateTimer()
    {
        if(_canAttack) return;

        _attackTimer -= Time.deltaTime;

        if (_attackTimer <= 0f)
        {
            _attackTimer = attackSpeed;
            _canAttack = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.color = Color.red;

        Matrix4x4 previousMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(
            attackPoint.position,
            attackPoint.rotation,
            Vector3.one
        );

        Gizmos.DrawWireCube(Vector3.zero, attackSize);
        Gizmos.matrix = previousMatrix;
    }
}
