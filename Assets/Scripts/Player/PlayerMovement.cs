using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player variables")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Renderer renderer;

    [Header("movement variables")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private ParticleSystem _movementParticles;

    private PlayerConfiguration _configuration;
    private InputAction _movementAction;
    private Vector2 _movementInput;

    public bool IsPerformingAction;
    private ParticleSystem.EmissionModule _emission;

    void Start()
    {
        _movementAction = playerInput.currentActionMap.FindAction("Move");
        _emission = _movementParticles.emission;
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (_movementInput == Vector2.zero)
        {
            _emission.enabled = false; // when stopped
            return;
        }

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

        _emission.enabled = true;  // when moving       

        //_movementParticles.Play();
        controller.Move(new Vector3(transform.forward.x, movement.y, transform.forward.z) * movement.magnitude * Time.deltaTime);
    }

    public void InitializePlayer(PlayerConfiguration pc)
    {
        _configuration = pc;
        renderer.material = pc.PlayerMaterial;
        _configuration.Input.onActionTriggered += Input_onActionTriggered1;
    }

    private void Input_onActionTriggered1(InputAction.CallbackContext obj)
    {
        if (obj.action != _configuration.Input.currentActionMap.FindAction("Move")) return;
        _movementInput = obj.ReadValue<Vector2>();
    }
}
