using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerAction))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputHandler _input;
    private PlayerAction _action;

    void Awake()
    {
        _input = GetComponent<PlayerInputHandler>();
        _action = GetComponent<PlayerAction>();
    }
    
    private void OnEnable()
    {
        _input.Move += _action.OnMove;
    }

    private void OnDisable()
    {
        _input.Move -= _action.OnMove;
    }
}