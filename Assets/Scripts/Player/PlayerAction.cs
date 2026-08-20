using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAction : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpPower = 12f;

    [Range(0f, 1f)]
    [SerializeField] private float jumpCutMultiplier = 0.5f;

    [SerializeField] private float coyoteTime = 0.15f;

    [Tooltip("1: 일반 점프, 2: 2단 점프")]
    [Min(1)]
    [SerializeField] private int maxJumpCount = 1;
    [SerializeField] private float jumpBufferTime = 0.15f;


    [Header("Move")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 40f;

    private Vector2 _movement;
    private Rigidbody2D _rb;

    private bool _isGrounded;
    private bool _hasTouchedGround;

    private float _coyoteTimer;
    private int _remainingAirJumps;
    private float _jumpBufferTimer;

    private bool _useDash = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        UpdateCoyoteTime();
        UpdateJumpBuffer();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void GetData(PlayerStat data)
    {
        jumpPower = data.jumpPower;
        speed = data.Speed;
    }

    public void SetMovementLocked(bool locked)
    {
        _useDash = locked;
    }

    public void OnMove(Vector2 value)
    {
        _movement = value;
    }

    public void SetGrounded(bool isGrounded)
    {
        bool hasLanded = !_isGrounded && isGrounded;

        _isGrounded = isGrounded;

        if (!hasLanded)
            return;

        _hasTouchedGround = true;
        _remainingAirJumps = maxJumpCount - 1;
        _coyoteTimer = coyoteTime;
        
        if (_jumpBufferTimer > 0f)
        {
            TryJump();
        }
    }

    public void OnJump()
    {
        if (_useDash) return;
        _jumpBufferTimer = jumpBufferTime;
        TryJump();
    }

    public void OnReleaseJump()
    {
        if (_rb.linearVelocity.y <= 0f)
            return;

        _rb.linearVelocity = new Vector2(
            _rb.linearVelocity.x,
            _rb.linearVelocity.y * jumpCutMultiplier
        );
    }

    private void UpdateCoyoteTime()
    {
        if (_isGrounded)
        {
            _coyoteTimer = coyoteTime;
            return;
        }

        _coyoteTimer = Mathf.Max(
            0f,
            _coyoteTimer - Time.deltaTime
        );
    }
    
    private void UpdateJumpBuffer()
    {
        _jumpBufferTimer = Mathf.Max(
            0f,
            _jumpBufferTimer - Time.deltaTime
        );
    }
    
    private void TryJump()
    {
        bool canGroundJump =
            _hasTouchedGround &&
            (_isGrounded || _coyoteTimer > 0f);

        bool canAirJump =
            !canGroundJump &&
            _remainingAirJumps > 0;

        if (!canGroundJump && !canAirJump)
            return;

        if (canAirJump)
            _remainingAirJumps--;

        _rb.linearVelocity = new Vector2(
            _rb.linearVelocity.x,
            jumpPower
        );

        _coyoteTimer = 0f;
        _isGrounded = false;
        _jumpBufferTimer = 0f;
    }

    private void Move()
    {
        if (_useDash) return;
        
        float targetX = _movement.x * speed;

        float changeSpeed = _movement.x == 0f
            ? deceleration
            : acceleration;

        float newX = Mathf.MoveTowards(
            _rb.linearVelocity.x,
            targetX,
            changeSpeed * Time.fixedDeltaTime
        );

        _rb.linearVelocity = new Vector2(
            newX,
            _rb.linearVelocity.y
        );
    }
}