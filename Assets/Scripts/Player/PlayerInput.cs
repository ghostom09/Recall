using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<Vector2> MoveChanged;
    public event Action JumpPressed;
    public event Action JumpReleased;
    public event Action Attack;
    public event Action Dash;
    public event Action Return;

    private InputSystem_Actions _input;

    private void Awake()
    {
        _input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _input.Player.Enable();

        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMove;

        _input.Player.Jump.started += OnJump;
        _input.Player.Jump.canceled += OnJump;

        _input.Player.Attack.performed += OnAttack;
        
        _input.Player.Dash.performed += OnDash;
        _input.Player.DashReturn.performed += OnReturn;
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMove;

        _input.Player.Jump.started -= OnJump;
        _input.Player.Jump.canceled -= OnJump;

        _input.Player.Attack.performed -= OnAttack;
        
        _input.Player.Dash.performed -= OnDash;
        _input.Player.DashReturn.performed -= OnReturn;

        _input.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveChanged?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpPressed?.Invoke();
        }
        else if (context.canceled)
        {
            JumpReleased?.Invoke();
        }
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Attack?.Invoke();
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        Dash?.Invoke();
    }

    private void OnReturn(InputAction.CallbackContext context)
    {
        Return?.Invoke();
    }
}
