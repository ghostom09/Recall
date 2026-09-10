using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Vector2 fireOffset = new Vector2(0.6f, 0f);

    private float direction;

    public override void Attack(float direction)
    {
        this.direction = direction;
        Vector2 attackPosition = (Vector2)transform.position +
            new Vector2(fireOffset.x * direction, fireOffset.y);

        if (bulletPrefab != null)
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, direction > 0f ? 0f : 180f);
            GameObject effect = Instantiate(bulletPrefab, attackPosition, rotation);
            Destroy(effect, 0.5f);
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * direction,
            data.attackRange, playerLayer | groundLayer);
        if (hit.collider == null) return;

        IDamageable target = hit.collider.GetComponentInParent<IDamageable>();
        if (target != null)
        {
            target.TakeDamage(data.Damage);
        }
    }

    public override void Move(float direction)
    {
        
    }

    public override bool TryRetreat()
    {
        if (health.Current <= health.Max / 2)
        {
            Rb.linearVelocity = new Vector2(-direction * speed, Rb.linearVelocity.y);
            
            return true;
        }
        
        return false;
    }
}
