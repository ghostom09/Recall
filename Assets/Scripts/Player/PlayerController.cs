using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private PlayerGroundChecker groundChecker;
    [SerializeField] private PlayerStat data;
    [SerializeField] private Health health;
    [SerializeField] private PlayerAction playerAction;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerSkill playerSkill;

    private void Awake()
    {
        health.Initialize(data.MaxHealth);
        playerAttack.GetAttackDamage(data.Damage);
    }

    private void OnEnable()
    {
        inputHandler.MoveChanged += playerAction.OnMove;
        inputHandler.MoveChanged += playerSkill.OnMove;
        inputHandler.JumpPressed += playerAction.OnJump;
        inputHandler.JumpReleased += playerAction.OnReleaseJump;
        inputHandler.Attack += playerAttack.OnAttack;
        inputHandler.Dash += playerSkill.OnDash;
        inputHandler.Return += playerSkill.OnReturn;

        groundChecker.GroundedChanged += playerAction.SetGrounded;
        
        health.Died += HandleDeath;
        
        playerSkill.DashStateChanged += playerAction.SetMovementLocked;

        playerAction.SetGrounded(groundChecker.IsGrounded);
    }

    private void OnDisable()
    {
        inputHandler.MoveChanged -= playerAction.OnMove;
        inputHandler.MoveChanged -= playerSkill.OnMove;
        inputHandler.JumpPressed -= playerAction.OnJump;
        inputHandler.JumpReleased -= playerAction.OnReleaseJump;
        inputHandler.Attack -= playerAttack.OnAttack;
        inputHandler.Dash -= playerSkill.OnDash;
        inputHandler.Return -= playerSkill.OnReturn;

        groundChecker.GroundedChanged -= playerAction.SetGrounded;
        
        health.Died -= HandleDeath;
        
        playerSkill.DashStateChanged -= playerAction.SetMovementLocked;
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }
}