using UnityEngine;
using UnityEngine.InputSystem;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private Transform holdPoint;
    [SerializeField] private BoxCollider collider;
    [SerializeField] private LayerMask toolLayer;
    [SerializeField] private FarmInteraction FarmInteraction;
    private InputAction _action;

    private GameObject _tool;
    private bool IsHoldingItem;
    private SeedIdentifier _heldSeedIdentifier;

    private void Start()
    {
        _action = input.currentActionMap.FindAction("Attack");
    }

    private void Update()
    {
        if (_action.WasPressedThisFrame())
        {
            Interact();
        }
    }

    private void Interact()
    {
        //get item
        if (!IsHoldingItem)
        {
            Collider[] overlaps = Physics.OverlapBox
            (
                collider.bounds.center,
                collider.transform.localScale,
                transform.rotation,
                toolLayer
            );

            if (overlaps.Length == 0)
            {
                FarmInteraction.Mode = InteractionMode.Idle;
                return;
            }

            _tool = overlaps[0].gameObject;
            _tool.transform.parent = holdPoint;
            _tool.transform.position = holdPoint.position;
            _tool.transform.rotation = holdPoint.rotation;
            _tool.GetComponent<Rigidbody>().isKinematic = true;
            IsHoldingItem = true;
            string type = overlaps[0].tag;

            switch (type)
            {
                case "Plow":
                    FarmInteraction.Mode = InteractionMode.Plowing;
                    break;
                case "Seed":
                    FarmInteraction.Mode = InteractionMode.Planting;
                    _heldSeedIdentifier = _tool.GetComponent<SeedIdentifier>();
                    break;
                case "Water":
                    FarmInteraction.Mode = InteractionMode.Watering;
                    break;
                case "Sickle":
                    FarmInteraction.Mode = InteractionMode.Harvesting;
                    break;
            }
        }

        //yeet item
        else
        {
            _tool.transform.parent = null;
            _tool.GetComponent<Rigidbody>().isKinematic = false;
            _tool.GetComponent<Rigidbody>().AddForce(transform.forward + transform.up * 2, ForceMode.Impulse);
            _tool = null;
            _heldSeedIdentifier = null;
            FarmInteraction.Mode = InteractionMode.Idle;
            IsHoldingItem = false;
        }
    }

    public SeedIdentifier GetHeldSeedIdentifier()
    {
        return _heldSeedIdentifier;
    }
}
