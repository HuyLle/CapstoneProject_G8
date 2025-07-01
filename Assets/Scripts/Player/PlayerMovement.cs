using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    [SerializeField] private float moveSpeed = 5f; // Tốc độ di chuyển
    [SerializeField] private float rotationSpeed = 720f; // Tốc độ xoay
    [SerializeField] private float gravity = -9.81f; // Trọng lực
    private Vector2 moveInput;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Xử lý di chuyển
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Dính sàn
        }
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        // Trọng lực
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Xoay
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Animation
        float speed = moveInput.magnitude;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsMoving", speed > 0.1f);
    }

    public void OnMoveInput(Vector2 input)
    {
        moveInput = input;
        Debug.Log(gameObject.name + " Move Input: " + input); // Debug input
    }
}