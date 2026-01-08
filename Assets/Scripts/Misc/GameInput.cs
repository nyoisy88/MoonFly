using System;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    public event EventHandler OnPauseAction;

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        Instance = this;
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
