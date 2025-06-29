using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerControls controls;
    private PlayerMovement movement;
    private PlayerInteraction interaction;

    void Awake()
    {
        controls = new PlayerControls();
        movement = GetComponent<PlayerMovement>();
        interaction = GetComponent<PlayerInteraction>();
    }

    void OnEnable()
    {
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
        controls.Player.Interact.performed += OnInteract;
        controls.Player.Enable();
        Debug.Log("Input System Enabled for " + gameObject.name);
    }

    void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Interact.performed -= OnInteract;
        controls.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        movement.OnMoveInput(context.ReadValue<Vector2>());
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        interaction.OnInteract();
        Debug.Log(gameObject.name + " Interact Pressed");
    }
}