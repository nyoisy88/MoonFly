using System;
using UnityEngine;

public class GameInput : Singleton<GameInput>
{

    public event EventHandler OnPauseAction;

    private InputSystem_Actions inputActions;

    protected override void Awake()
    {
        base.Awake();
        inputActions = new InputSystem_Actions();
        inputActions.Player.Move.Enable();
        inputActions.Player.Pause.Enable();
        inputActions.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        inputActions.Player.Move.Disable();
        inputActions.Dispose();
    }

    public Vector2 GetMovementInput()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

}
