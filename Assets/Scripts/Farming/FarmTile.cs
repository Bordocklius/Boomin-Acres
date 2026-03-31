using System;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    public event EventHandler WaterSwitched;

    [SerializeField] private Material[] materialStates;
    [SerializeField] private float plowingTime;
    [SerializeField] private float wateringTime;
    [SerializeField] private float harvestingTime;
    [SerializeField] private Renderer renderer;

    public bool IsPlowed { get; private set; }
    public bool IsWatered {  get; private set; }

    private float _timer;
    public float WateringTimer = 10f;

    public bool IsReadyToHarvest {  get; private set; }

    private void Start()
    {
        IsPlowed = false;
        IsWatered = false;
        IsReadyToHarvest = false;
    }
    /*
    public IEnumerator Plowing()
    {
        if (FarmState != TileState.Barren)
        {
            Debug.Log("Not barren");
            yield break;
        }

        yield return new WaitForSeconds(plowingTime);
        IsPlowed = true;
        renderer.material = materialStates[1];
        FarmState = TileState.Plowed;
    }

    public IEnumerator Planting()
    {
        if (FarmState != TileState.Plowed)
        {
            Debug.Log("not plowed");
            yield break;
        }

        yield return new WaitForSeconds(plowingTime);
        IsPlowed = true;
        renderer.material = materialStates[1];
        FarmState = TileState.Plowed;
    }

    public IEnumerator Watering()
    {
        if (FarmState != TileState.Plowed)
        {
            Debug.Log("not planted");
            yield break;
        }

        yield return new WaitForSeconds(wateringTime);
        IsWatered = true;
        IsReadyToHarvest = true;
        renderer.material = materialStates[2];
        FarmState = TileState.Watered;
    }

    public IEnumerator Harvesting()
    {
        if (FarmState != TileState.Watered)
        {
            Debug.Log("Crop needs to grow first!");
            yield break;
        }

        yield return new WaitForSeconds(harvestingTime);

        IsPlowed = false;
        IsWatered = false;
        IsReadyToHarvest= false;
        renderer.material = materialStates[0];
    }
    */

    public void Update()
    {
        UpdateWaterTimer();
    }

    private void UpdateWaterTimer()
    {
        if (!IsWatered)
            return;

        _timer += Time.deltaTime;
        if (_timer > WateringTimer)
        {
            IsWatered = false;
        }
    }

    public void PlowPlot()
    {
        IsPlowed = true;
        renderer.material = materialStates[1];
    }

    public void WaterPlot()
    {
        IsWatered = true;
        renderer.material = materialStates[2];
        _timer = 0f;
    }

    public void ResetPlot()
    {
        IsPlowed = false;
        IsWatered = false;
        renderer.material = materialStates[0];
    }
}
