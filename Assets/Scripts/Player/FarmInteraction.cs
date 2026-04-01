using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private PlayerInput input;
    [SerializeField] private float holdInterval = 0.5f;

    public InteractionMode Mode = InteractionMode.Idle;

    private InputAction _action;
    private ItemHolder _itemHolder;

    private FarmTile _tile;
    [SerializeField] private float _holdTimer;
    private bool held;

    private void Start()
    {
        _action = input.currentActionMap.FindAction("Interact");
        _itemHolder = GetComponent<ItemHolder>();
        _progressSlider.gameObject.SetActive(false);
    }

    //detecting tiles
    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position - transform.up, -transform.up);
        Debug.DrawRay(transform.position - transform.up, -transform.up, Color.red);
        if (Physics.Raycast(ray, out RaycastHit hit, .1f))
        {
            if (hit.transform.tag == "Tile")
            {
                if (_tile == null)
                {
                    Debug.Log("new tile");
                    _tile = hit.transform.gameObject.GetComponent<FarmTile>();
                    return;
                }
                else if (hit.transform.gameObject.GetComponent<FarmTile>() != _tile)
                {
                    _tile = hit.transform.gameObject.GetComponent<FarmTile>();
                    Debug.Log("replaced tile"); 
                }
            }
        }
        else _tile = null;
    }

    void Update()
    {
        _action.started += _ => held = true;
        _action.canceled += _ => held = false;

        Sliderhandler();

        //determine what action is used
        if (_tile != null)
        {
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
    }

    void Plowing()
    {
        if (held && _holdTimer >= holdInterval)
        {
            Debug.Log("p1");
            _tile.PlowPlot();
            return;
        }

        if (_action.WasReleasedThisFrame())
        {
            if (_holdTimer < holdInterval)
            {
                Debug.Log("p2");
                _tile.PlowPlot();
            }
            return;
        }
    }

    void Planting()
    {
        if (held && _holdTimer >= holdInterval)
        {
            SeedIdentifier seedIdentifier = _itemHolder.GetHeldSeedIdentifier();
            if (seedIdentifier != null)
            {
                CropSO cropData = CropManager.Instance.GetCropByName(seedIdentifier.CropName);
                if (cropData != null)
                {
                    _tile.PlantPlot(cropData);
                    Debug.Log($"Planted {seedIdentifier.CropName}");
                }
            }
            return;
        }

        if (_action.WasReleasedThisFrame())
        {
            if (_holdTimer < holdInterval)
            {
                SeedIdentifier seedIdentifier = _itemHolder.GetHeldSeedIdentifier();
                if (seedIdentifier != null)
                {
                    CropSO cropData = CropManager.Instance.GetCropByName(seedIdentifier.CropName);
                    if (cropData != null)
                    {
                        _tile.PlantPlot(cropData);
                        Debug.Log($"Planted {seedIdentifier.CropName}");
                    }
                }
            }
            return;
        }
    }

    void Watering()
    {
        if (held && _holdTimer >= holdInterval)
        {
            Debug.Log("w1");
            _tile.WaterPlot();
            return;
        }

        if (_action.WasReleasedThisFrame())
        {
            if (_holdTimer < holdInterval)
            {
                Debug.Log("w2");
                _tile.WaterPlot();
            }
            return;
        }
    }

    void Harvesting()
    {
        if (held && _holdTimer >= holdInterval)
        {
            Debug.Log("h1");
            _tile.ResetPlot();
            PlantedCrop crop = _tile.PlantedCrop;
            if(crop != null)
                crop.RequestHarvest();
            return;
        }

        if (_action.WasReleasedThisFrame())
        {
            if (_holdTimer < holdInterval)
            {
                Debug.Log("h2");
                _tile.ResetPlot();
                PlantedCrop crop = _tile.PlantedCrop;
                if (crop != null)
                    crop.RequestHarvest();
            }
            return;
        }
    }

    void Sliderhandler()
    {
        if (Mode == InteractionMode.Idle) return;

        if (held && _holdTimer >= holdInterval)
        {
            _progressSlider.gameObject.SetActive(false); 
            return;
        }

        if (held && _holdTimer < holdInterval)
        {
            if (_holdTimer > 0.1f)
            {
                _progressSlider.gameObject.SetActive(true);
            }
            _progressSlider.value = _holdTimer / holdInterval;
            _holdTimer += Time.deltaTime;
            return;
        }

        _progressSlider.gameObject.SetActive(false);
        _holdTimer = 0;
    }
}
