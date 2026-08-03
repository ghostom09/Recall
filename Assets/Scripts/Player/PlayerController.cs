using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private PlayerGroundChecker groundChecker;
    [SerializeField] private PlayerAction playerAction;

    private void OnEnable()
    {
        inputHandler.MoveChanged += playerAction.OnMove;
        inputHandler.JumpPressed += playerAction.OnJump;
        inputHandler.JumpReleased += playerAction.OnReleaseJump;

        groundChecker.GroundedChanged += playerAction.SetGrounded;

        playerAction.SetGrounded(groundChecker.IsGrounded);
    }

    private void OnDisable()
    {
        inputHandler.MoveChanged -= playerAction.OnMove;
        inputHandler.JumpPressed -= playerAction.OnJump;
        inputHandler.JumpReleased -= playerAction.OnReleaseJump;

        groundChecker.GroundedChanged -= playerAction.SetGrounded;
    }
}