using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAction : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float acceleration = 20f;
    [SerializeField] private float deceleration = 40f;
    
    private Vector2 _movement;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    public void OnMove(Vector2 value)
    {
        _movement = value;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector2 targetVelocity = _movement * speed;

        float changeSpeed = _movement == Vector2.zero
            ? deceleration
            : acceleration;

        _rb.linearVelocity = Vector2.MoveTowards(
            _rb.linearVelocity,
            targetVelocity,
            changeSpeed * Time.fixedDeltaTime
        );
    }
}