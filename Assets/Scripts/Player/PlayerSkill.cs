using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private float dashPower = 2f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private PlayerCameraController playerCameraController;
    [SerializeField] private float dashCoolTime = 3f;
    
    private Rigidbody2D _rb;
    private Vector2 _startPos;
    private float _dir;
    private float _lastDirection = 1f;
    private float _dashDirection;
    private float _velocityXBeforeDash;
    private float _dashTimer;
    private bool _isDashing;
    
    public event Action<bool> DashStateChanged; 

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
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
        if (_isDashing)
            return;

        _dashDirection = _lastDirection;
        _velocityXBeforeDash = _rb.linearVelocity.x;
        _startPos = transform.position;
        _dashTimer = dashDuration;
        _isDashing = true;

        DashStateChanged?.Invoke(true);
    }

    private void EndDash()
    {
        _isDashing = false;
        DashStateChanged?.Invoke(false);

        _rb.linearVelocity = new Vector2(
            _velocityXBeforeDash,
            _rb.linearVelocity.y
        );
    }

    public void OnReturn()
    {
        transform.position = _startPos;
        playerCameraController.OnPlayerTeleported();
    }
}
