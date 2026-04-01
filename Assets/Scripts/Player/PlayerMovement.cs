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
    //private InputAction _movementAction;
    private Vector2 _movementInput;

    private Vector3 _velocity;

    public bool IsPerformingAction;
    private ParticleSystem.EmissionModule _emission;

    void Start()
    {
        //_movementAction = playerInput.currentActionMap.FindAction("Move");
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
            _velocity.y += Physics.gravity.y * Time.deltaTime;
        }
        else _velocity.y = 0f;

        Vector3 XZVelocity = new Vector3(transform.forward.x, 0, transform.forward.z) * movement.magnitude;
        _velocity = new Vector3(XZVelocity.x, _velocity.y, XZVelocity.z);
        _emission.enabled = true;  // when moving       

        //_movementParticles.Play();
        controller.Move(_velocity * Time.deltaTime);
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

    public void AddVelocity(Vector3 veloity)
    {
        _velocity += veloity;
    }
}
