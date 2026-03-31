using UnityEngine;
using UnityEngine.InputSystem;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 500f;

    private PlayerInput playerInput;
    private RectTransform rectTransform;
    private InputAction moveAction;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Navigate"];
    }

    void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        rectTransform.anchoredPosition += move * moveSpeed * Time.deltaTime;
    }
}
