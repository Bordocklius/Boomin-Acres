using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController controller;

    [Header("movement variables")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float rotationSpeed;

    private InputAction _movementAction;

    public bool IsPerformingAction;

    void Start()
    {
        _movementAction = playerInput.currentActionMap.FindAction("Move"); 
    }

    void Update()
    {
        if (_movementAction == null) return;

        Vector2 newInput = _movementAction.ReadValue<Vector2>();
        Vector3 moveVelocity = new Vector3(newInput.x, 0f, newInput.y) * playerSpeed;
        if (IsPerformingAction) moveVelocity = moveVelocity / 2;

        // Rotate to face movement direction
        if (moveVelocity.magnitude > 0.01f) // Only if actually moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVelocity, Vector3.up);
            if (IsPerformingAction) transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / 3 * Time.deltaTime);
            else transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 XZMovement = moveVelocity;

        if (!controller.isGrounded)
        {
            moveVelocity += Physics.gravity;
        }

        controller.Move(new Vector3(transform.forward.x, moveVelocity.y, transform.forward.z) * XZMovement.magnitude * Time.deltaTime);
    }
}
