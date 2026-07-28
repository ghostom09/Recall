using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<Vector2> Move;
    
    private InputSystem_Actions _input;

    private void Awake()
    {
        _input = new();
    }

    private void OnEnable()
    {
        _input.Player.Enable();
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var move = context.ReadValue<Vector2>();
        Move?.Invoke(move);
    }

    private void OnDisable()
    {
        _input.Player.Move.performed -= OnMove;
        _input.Player.Move.canceled -= OnMove;
        _input.Player.Disable();
    }
    
    private void OnDestroy()
    {
        _input.Dispose();
    }
}
