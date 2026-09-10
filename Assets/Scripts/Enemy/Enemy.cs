using UnityEngine;
using System.Collections.Generic;

public abstract class Enemy : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected EnemyStat data;
    [SerializeField] protected Health health;
    [Header("Movement")]
    [SerializeField] protected LayerMask groundLayer;
    [Header("Attack")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private GameObject attackEffect;
    private float _attackTimer;
    private int _previousHealth;
    private Collider2D _bodyCollider;
    protected Rigidbody2D Rb { get; private set; }
    public EnemyStateMachine StateMachine { get; private set; } = new ();
    
    
    private Transform _target;
    public int speed
    {
        get { return data.Speed; }
        private set { data.Speed = value; }
    }

    #region FSMReferences
    public IdleState IdleState { get; private set; }
    public MoveState MoveState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public AttackState AttackState { get; private set; }
    public HurtState HurtState { get; private set; }
    
    #endregion

    private void Awake()
    {
        
        health.Initialize(data.MaxHealth);
        _previousHealth = health.Current;
        Rb = GetComponent<Rigidbody2D>();
        _bodyCollider = GetComponent<Collider2D>();
        if (groundLayer.value == 0)
            groundLayer = LayerMask.GetMask("Ground");
        if (playerLayer.value == 0)
            playerLayer = LayerMask.GetMask("Player");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _target = player != null ? player.transform : null;
        
        IdleState = new IdleState(this);
        MoveState = new MoveState(this);
        ChaseState = new ChaseState(this);
        AttackState = new AttackState(this);
        HurtState = new HurtState(this);
    }

    private void Start()
    {
        StateMachine.ChangeState(IdleState);
    }

    private void OnEnable()
    {
        health.Died += HandleDeath;
        health.HealthChanged += HandleHurt;
    }

    private void FixedUpdate()
    {
        if (health.IsDead) return;
        _attackTimer -= Time.fixedDeltaTime;
        StateMachine.Update();
    }

    private void OnDisable()
    {
        health.Died -= HandleDeath;
        health.HealthChanged -= HandleHurt;
    }

    private void HandleHurt(int current, int max)
    {
        bool tookDamage = current < _previousHealth;
        _previousHealth = current;
        if (tookDamage && !health.IsDead)
        {
            StateMachine.ChangeState(HurtState);
        }
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }

    #region ForFSM

    public bool CanChase => _target != null &&
        Vector2.Distance(transform.position, _target.position) <= data.detectionRange;

    public float TargetDeltaX => _target != null ? _target.position.x - transform.position.x : 0f;
    public float StopDistance => Mathf.Max(0.1f, data.attackRange);
    public bool CanAttack => _target != null &&
        Vector2.Distance(transform.position, _target.position) <= data.attackRange;
    public bool AttackReady => _attackTimer <= 0f;

    public virtual bool TryRetreat()
    {
        return false;   
    }
    

    public void StartAttackCooldown()
    {
        _attackTimer = attackCooldown;
    }

    public virtual void Attack(float direction)
    {
        Vector2 attackPosition = (Vector2)transform.position +
            direction * data.attackRange * 0.5f * Vector2.right;
        if (attackEffect != null)
        {
            GameObject effect = Instantiate(attackEffect, attackPosition, Quaternion.identity, transform);
            Destroy(effect, 0.5f);
        }

        Vector2 attackSize = new Vector2(data.attackRange, 1f);
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPosition, attackSize, 0f, playerLayer);
        HashSet<IDamageable> damagedTargets = new();
        foreach (Collider2D hit in hits)
        {
            IDamageable target = hit.GetComponentInParent<IDamageable>();
            if (target == null || !damagedTargets.Add(target)) continue;
            target.TakeDamage(data.Damage);
        }
    }

    public virtual void Move(float direction)
    {
        Rb.linearVelocity = new Vector2(direction * speed, Rb.linearVelocity.y);
    }

    public void StopMoving()
    {
        Rb.linearVelocity = new Vector2(0f, Rb.linearVelocity.y);
    }

    public bool CanMove(float direction)
    {
        Bounds bounds = _bodyCollider.bounds;
        float ahead = bounds.extents.x + speed * Time.fixedDeltaTime + 0.1f;
        Vector2 origin = new Vector2(
            bounds.center.x + direction * ahead,
            bounds.min.y + 0.1f);

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.4f, groundLayer);
        Debug.DrawRay(origin, Vector2.down * 0.4f, Color.green);
        return hit.collider != null && !hit.collider.isTrigger;
    }

    #endregion
}
