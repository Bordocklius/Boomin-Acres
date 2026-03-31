using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InteractionMode
{
    Idle,
    Plowing,
    Planting,
    Watering,
    Harvesting
}

public class FarmInteraction : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private float holdInterval = 0.5f;

    public InteractionMode Mode = InteractionMode.Idle;

    private InputAction _action;

    private bool HasPressed;
    private bool held;
    private float _holdTimer;

    private void Start()
    {
        _action = input.currentActionMap.FindAction("Interact");
    }

    void Update()
    {
        _action.started += _ => held = true;
        _action.canceled += _ => held = false;
        
        if (held && _holdTimer < holdInterval)
        {
            _holdTimer += Time.deltaTime;
        }

        switch (Mode)
        {
            case InteractionMode.Idle:
                break;
            case InteractionMode.Plowing:
                Plowing();
                break;
            case InteractionMode.Planting:
                Planting();
                break;
            case InteractionMode.Watering:
                Watering();
                break;
            case InteractionMode.Harvesting:
                Harvesting();
                break;
        }
    }

    void Plowing()
    {
        if (held && _holdTimer >= holdInterval)
        {
            Debug.Log("MultiPlowing");
            return;
        }

        if (_action.WasReleasedThisFrame())
        {
            if (_holdTimer < holdInterval)
            {
                Debug.Log("SoloPlowing");
            }
            _holdTimer = 0;
            return;
        }
    }

    void Planting()
    {
        Debug.Log("no");
    }

    void Watering()
    {
        Debug.Log("no");
    }

    void Harvesting()
    {
        Debug.Log("no");
    }
}
