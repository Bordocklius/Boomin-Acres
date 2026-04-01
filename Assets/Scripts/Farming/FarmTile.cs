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
    public bool IsWatered { get; private set; }

    private float _timer;
    public float WateringTimer = 10f;

    public bool IsReadyToHarvest { get; private set; }

    private void Start()
    {
        IsPlowed = false;
        IsWatered = false;
        IsReadyToHarvest = false;
    }

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
        if (IsPlowed) return;
        IsPlowed = true;
        renderer.material = materialStates[1];
    }

    public void WaterPlot()
    {
        if (IsWatered || !IsPlowed) return;
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
