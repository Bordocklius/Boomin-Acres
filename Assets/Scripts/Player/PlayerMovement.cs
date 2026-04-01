using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CharacterController controller;

    [Header("movement variables")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private ParticleSystem _movementParticles;

    private InputAction _movementAction;
    private Vector2 _movementInput;

    public bool IsPerformingAction;

    void Start()
    {
        _movementAction = playerInput.currentActionMap.FindAction("Move"); 
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_movementInput == Vector2.zero)
            return;

        Vector3 movement = new Vector3(_movementInput.x, 0f, _movementInput.y) * playerSpeed;
        if (IsPerformingAction) movement = movement / 2;

        if(movement.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            if (IsPerformingAction) transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / 3 * Time.deltaTime);
            else transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);            
        }

        if (!controller.isGrounded)
        {
            movement += Physics.gravity;
        }

        _movementParticles.Play();
        controller.Move(new Vector3(transform.forward.x, movement.y, transform.forward.z) * movement.magnitude * Time.deltaTime);
    }

    public void OnMove(InputValue value)
    {
        _movementInput = value.Get<Vector2>();
    }
}
