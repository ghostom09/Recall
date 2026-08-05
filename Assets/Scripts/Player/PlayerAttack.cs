using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Transform attackPivot;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Vector2 attackSize = new(2f, 1f);
    [SerializeField] private LayerMask enemyLayer;

    private bool _isFacingRight = true;
    private int _damage;

    public void GetAttackDamage(int damage)
    {
        _damage = damage;
    }

    public void OnAttack()
    {
        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition =
            Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        Vector2 direction =
            (mouseWorldPosition - (Vector2)transform.position).normalized;
        
        UpdateAttackDirection(direction.x);
        ApplyDamage();
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

    private void ApplyDamage()
    {
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