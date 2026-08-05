using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private PlayerGroundChecker groundChecker;
    [SerializeField] private PlayerStat data;
    [SerializeField] private Health health;
    [SerializeField] private PlayerAction playerAction;

    private void Awake()
    {
        health.Initialize(data.MaxHealth);
    }

    private void OnEnable()
    {
        inputHandler.MoveChanged += playerAction.OnMove;
        inputHandler.JumpPressed += playerAction.OnJump;
        inputHandler.JumpReleased += playerAction.OnReleaseJump;

        groundChecker.GroundedChanged += playerAction.SetGrounded;
        
        health.Died += HandleDeath;

        playerAction.SetGrounded(groundChecker.IsGrounded);
    }

    private void OnDisable()
    {
        inputHandler.MoveChanged -= playerAction.OnMove;
        inputHandler.JumpPressed -= playerAction.OnJump;
        inputHandler.JumpReleased -= playerAction.OnReleaseJump;

        groundChecker.GroundedChanged -= playerAction.SetGrounded;
    }
   
    private void OnDestroy()
    {
        health.Died -= HandleDeath;
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }
}