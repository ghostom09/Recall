using System;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    public event Action<bool> GroundedChanged;

    public bool IsGrounded { get; private set; }

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;

    [Header("Ground Check")]
    [SerializeField] private Vector2 checkSize = new Vector2(0.7f, 0.15f);
    [SerializeField] private LayerMask groundLayer;

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        bool isTouchingGround = Physics2D.OverlapBox(
            groundCheck.position,
            checkSize,
            0f,
            groundLayer
        );
        
        bool newIsGrounded =
            isTouchingGround &&
            rb.linearVelocity.y <= 0.1f;

        if (newIsGrounded == IsGrounded)
            return;

        IsGrounded = newIsGrounded;
        GroundedChanged?.Invoke(IsGrounded);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireCube(groundCheck.position, checkSize);
    }
}