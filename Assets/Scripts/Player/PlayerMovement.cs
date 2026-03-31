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

    void Start()
    {
        _movementAction = playerInput.currentActionMap.FindAction("Move"); 
    }

    void Update()
    {
        if (_movementAction == null) return;
        Vector2 newInput = _movementAction.ReadValue<Vector2>();
        Vector3 moveVelocity = new Vector3(newInput.x, 0f, newInput.y) * playerSpeed;
        controller.Move(moveVelocity * Time.deltaTime);

        // Rotate to face movement direction
        if (moveVelocity.magnitude > 0.01f) // Only if actually moving
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveVelocity, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
