using System.Collections;
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
    [SerializeField] private PlayerCameraController playerCameraController;

    [Header("Attack Movement")]
    [Range(0f, 1f)]
    [SerializeField] private float attackMoveMultiplier = 0.35f;
    [SerializeField] private float attackMoveSlowDuration = 0.2f;

    private Coroutine _attackSlowCoroutine;

    private void Awake()
    {
        health.Initialize(data.MaxHealth);
        playerAttack.GetAttackDamage(data.Damage);
        playerAction.GetData(data);
        playerSkill.GetData(data);
    }

    private void OnEnable()
    {
        inputHandler.MoveChanged += playerAction.OnMove;
        inputHandler.MoveChanged += playerSkill.OnMove;
        inputHandler.JumpPressed += playerAction.OnJump;
        inputHandler.JumpReleased += playerAction.OnReleaseJump;
        inputHandler.Attack += HandleAttack;
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
        inputHandler.Attack -= HandleAttack;
        inputHandler.Dash -= playerSkill.OnDash;
        inputHandler.Return -= playerSkill.OnReturn;

        groundChecker.GroundedChanged -= playerAction.SetGrounded;
        
        health.Died -= HandleDeath;
        
        playerSkill.DashStateChanged -= playerAction.SetMovementLocked;

        if (_attackSlowCoroutine != null)
        {
            StopCoroutine(_attackSlowCoroutine);
            _attackSlowCoroutine = null;
        }

        playerAction.SetMoveSpeedMultiplier(1f);
    }

    private void HandleAttack()
    {
        if (!playerAttack.TryAttack())
            return;

        if (_attackSlowCoroutine != null)
        {
            StopCoroutine(_attackSlowCoroutine);
        }

        CameraShake(3f);
        _attackSlowCoroutine = StartCoroutine(AttackMovementSlow());
    }

    private void HandleSkill()
    {
        
    }

    private IEnumerator AttackMovementSlow()
    {
        playerAction.SetMoveSpeedMultiplier(attackMoveMultiplier);

        yield return new WaitForSeconds(attackMoveSlowDuration);

        playerAction.SetMoveSpeedMultiplier(1f);
        _attackSlowCoroutine = null;
    }

    private void CameraShake(float power)
    {
        playerCameraController.CameraShake(power);
    }

    private void HandleDeath()
    {
        Destroy(gameObject);
    }
}
