using System.Collections;
using UnityEngine;

public class FarmTile : MonoBehaviour
{
    [SerializeField] private Material[] materialStates;
    [SerializeField] private float plowingTime;
    [SerializeField] private float wateringTime;
    [SerializeField] private float harvestingTime;
    [SerializeField] private Renderer renderer;

    public bool IsPlowed { get; private set; }
    public bool IsWatered {  get; private set; }
    public bool IsReadyToHarvest {  get; private set; }

    private void Start()
    {
        IsPlowed = false;
        IsWatered = false;
        IsReadyToHarvest = false;
    }

    public IEnumerator Plowing()
    {
        if (IsPlowed)
        {
            Debug.Log("Already plowed");
            yield break;
        }

        yield return new WaitForSeconds(plowingTime);
        IsPlowed = true;
        renderer.material = materialStates[1];
    }

    public IEnumerator Watering()
    {
        if (!IsPlowed)
        {
            Debug.Log("Plow first!");
            yield break;
        }

        if (IsWatered)
        {
            Debug.Log("Already watered");
            yield break;
        }

        yield return new WaitForSeconds(wateringTime);
        IsWatered = true;
        IsReadyToHarvest = true;
        renderer.material = materialStates[2];
    }

    public IEnumerator Harvesting()
    {
        if (!IsReadyToHarvest)
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
}
