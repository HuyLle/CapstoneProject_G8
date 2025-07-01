using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private bool isPlayer1; // True nếu là Player1, False nếu là Player2
    private PlayerMovement movement; // Component di chuyển
    private PlayerInteraction interaction; // Component tương tác

    void Awake()
    {
        // Xác định người chơi dựa trên tên GameObject
        isPlayer1 = gameObject.name == "Player1";

        // Lấy component PlayerMovement
        movement = GetComponent<PlayerMovement>();
        if (movement == null)
        {
            Debug.LogError($"[{gameObject.name}] PlayerMovement component is missing!");
            enabled = false;
            return;
        }

        // Lấy component PlayerInteraction
        interaction = GetComponent<PlayerInteraction>();
        if (interaction == null)
        {
            Debug.LogError($"[{gameObject.name}] PlayerInteraction component is missing!");
            enabled = false;
            return;
        }

        Debug.Log($"[{gameObject.name}] Initialized as {(isPlayer1 ? "Player1 (Keyboard)" : "Player2 (GamePad)")}");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Lấy thiết bị gửi input
        var device = context.control.device;

        // Kiểm tra thiết bị và xử lý input
        if (isPlayer1 && device is Keyboard)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            // Debug.Log($"Player1 moving with Keyboard: {moveInput}"); // Có thể tắt bớt log để đỡ rối console
            movement.OnMoveInput(moveInput);
        }
        else if (!isPlayer1 && device is Gamepad)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            // Debug.Log($"Player2 moving with GamePad: {moveInput}"); // Có thể tắt bớt log để đỡ rối console
            movement.OnMoveInput(moveInput);
        }
        // else
        // {
        //     Debug.Log($"[{gameObject.name}] Ignored input from {device.name}");
        // }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Chỉ thực hiện hành động khi nút được nhấn xuống (performed)
        if (context.performed)
        {
            var device = context.control.device;

            if (isPlayer1 && device is Keyboard)
            {
                Debug.Log("Player1 interacting with Keyboard");
                interaction.OnInteract();
            }
            else if (!isPlayer1 && device is Gamepad)
            {
                Debug.Log("Player2 interacting with Gamepad");
                interaction.OnInteract();
            }
        }
    }
}