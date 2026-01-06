using UnityEngine;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private InputSystem_Actions inputActions;

    private void Awake()
    {
        Instance = this;
        inputActions = new InputSystem_Actions();
        inputActions.Player.Move.Enable();
    }

    public Vector2 GetMovementInput()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }

}
