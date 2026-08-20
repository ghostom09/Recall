using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private float dashPower = 25;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private PlayerCameraController playerCameraController;
    [SerializeField] private GameObject afterImage;
    [SerializeField] private LayerMask enemyLayer;
    private GameObject _tempAfterImage;
    
    private Rigidbody2D _rb;
    private Vector2 _startPos;
    private float _gravityScale;
    
    private float _dir;
    private float _lastDirection = 1f;
    private float _dashDirection;
    private float _velocityXBeforeDash;
    
    private float _dashTimer;
    private float _returnTimer;
    [SerializeField] private float returnTime = 3f;
    private float _cooldownTimer;
    private float _coolTime;
    
    private bool _isDashing;
    private bool _canReturn = false;
    
    public event Action<bool> DashStateChanged; 

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void FixedUpdate()
    {
        if(!_isDashing) return;
        
        _rb.linearVelocity = new Vector2(_dashDirection * dashPower, _rb.linearVelocity.y);
        
        _dashTimer -= Time.deltaTime;

        if (_dashTimer <= 0f)
        {
            EndDash();
        }
    }

    public void GetData(PlayerStat data)
    {
        _coolTime = data.DashCooldown;
    }

    public void OnMove(Vector2 pos)
    {
        _dir = pos.x;

        if (!Mathf.Approximately(_dir, 0f))
        {
            _lastDirection = Mathf.Sign(_dir);
        }
    }

    public void OnDash()
    {
        if (_isDashing || _cooldownTimer > 0f)
            return;

        _dashDirection = _lastDirection;
        _velocityXBeforeDash = _rb.linearVelocity.x;
        _startPos = transform.position;
        _tempAfterImage = Instantiate(afterImage, _startPos, Quaternion.identity);
        _dashTimer = dashDuration;
        _isDashing = true;
        _gravityScale = _rb.gravityScale;
        _rb.gravityScale = 0f;
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);

        DashStateChanged?.Invoke(true);
    }

    private void EndDash()
    {
        _isDashing = false;
        DashStateChanged?.Invoke(false);
        _rb.gravityScale = _gravityScale;

        _rb.linearVelocity = new Vector2(
            _velocityXBeforeDash,
            _rb.linearVelocity.y
        );
        
        _cooldownTimer = _coolTime;
        _returnTimer = returnTime;
        _canReturn = true;
    }

    public void OnReturn()
    {
        if (!_canReturn) return;
        _canReturn = false;
        transform.position = _startPos;

        Attack();
        
        if (_tempAfterImage)
        {
            Destroy(_tempAfterImage);
        }
        playerCameraController.OnPlayerTeleported();
    }

    
    private void Attack()
    {
        Vector2 start = transform.position;
        Vector2 end = _startPos;
        float height = 3f;
        
        Vector2 center = (start + end) / 2f;

        float width = Vector2.Distance(start, end);
        Vector2 size = new Vector2(width, height);

        Vector2 direction = end - start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle2 = angle;
        
        Collider2D[] hits = Physics2D.OverlapBoxAll(
            center,
            size,
            angle,
            enemyLayer
        );
        
        HashSet<IDamageable> damagedTargets = new();

        foreach (Collider2D hit in hits)
        {
            IDamageable target = hit.GetComponentInParent<IDamageable>();

            if (target == null || !damagedTargets.Add(target))
                continue;

            target.TakeDamage(1);
        }
    }

    private float angle2;

    private void UpdateTimer()
    {
        if (_cooldownTimer > 0f)
        {
            _cooldownTimer -= Time.deltaTime;
        }

        if (!_canReturn) return;

        _returnTimer -= Time.deltaTime;
        if (_returnTimer <= 0f)
        {
            _returnTimer = 0f;
            _canReturn = false;
            
            if (_tempAfterImage)
            {
                Destroy(_tempAfterImage);
            }
        }
    }
    
    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //
    //     Matrix4x4 previousMatrix = Gizmos.matrix;
    //
    //     Gizmos.matrix = Matrix4x4.TRS(
    //         start,
    //         angle2,
    //         Vector3.one
    //     );
    //
    //     Gizmos.DrawWireCube(Vector3.zero, attackSize);
    //     Gizmos.matrix = previousMatrix;
    // }
}
