using UnityEngine;
using UnityEngine.InputSystem;

public class TEMP_FarmTester : MonoBehaviour
{
    [SerializeField] private PlayerInput input;
    [SerializeField] private FarmTile tile;
    private InputAction _plow;
    private InputAction _water;
    private InputAction _harvest;

    private void Start()
    {
        _plow = input.currentActionMap.FindAction("Attack");
        _water = input.currentActionMap.FindAction("Interact");
        _harvest = input.currentActionMap.FindAction("Jump");
    }

    private void Update()
    {
        if (_plow.WasPressedThisFrame())
        {
            tile.PlowPlot();
        }

        if (_water.WasPressedThisFrame())
        {
            tile.WaterPlot();
        }

        if (_harvest.WasPressedThisFrame())
        {
            tile.ResetPlot();
        }
    }
}
