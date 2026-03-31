using UnityEngine;
using System.Collections.Generic;

public class CropManager : MonoBehaviour
{
    public static CropManager Instance { get; private set; }

    [SerializeField]
    private List<CropSO> availableCrops = new List<CropSO>();

    private Dictionary<string, CropSO> cropDictionary;
    private Dictionary<string, int> harvestedCrops;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        InitializeCropDictionary();
        InitializeHarvestedCrops();
    }

    private void InitializeCropDictionary()
    {
        cropDictionary = new Dictionary<string, CropSO>();
        foreach (CropSO crop in availableCrops)
        {
            if (!cropDictionary.ContainsKey(crop.CropName))
            {
                cropDictionary.Add(crop.CropName, crop);
            }
        }
    }

    private void InitializeHarvestedCrops()
    {
        harvestedCrops = new Dictionary<string, int>();
        foreach (CropSO crop in availableCrops)
        {
            harvestedCrops[crop.CropName] = 0;
        }
    }

    public CropSO GetCropByName(string cropName)
    {
        if (cropDictionary.TryGetValue(cropName, out CropSO crop))
        {
            return crop;
        }

        Debug.LogWarning($"Crop '{cropName}' not found in CropManager");
        return null;
    }

    public List<CropSO> GetAllCrops()
    {
        return new List<CropSO>(availableCrops);
    }

    public void AddHarvestedCrop(string cropName, int amount = 1)
    {
        if (harvestedCrops.ContainsKey(cropName))
        {
            harvestedCrops[cropName] += amount;
        }
        else
        {
            Debug.LogWarning($"Crop '{cropName}' not found in harvested crops inventory");
        }
    }

    public int GetHarvestedCropAmount(string cropName)
    {
        if (harvestedCrops.TryGetValue(cropName, out int amount))
        {
            return amount;
        }

        Debug.LogWarning($"Crop '{cropName}' not found in harvested crops inventory");
        return 0;
    }

    public bool TryRemoveHarvestedCrop(string cropName, int amount = 1)
    {
        if (harvestedCrops.TryGetValue(cropName, out int currentAmount))
        {
            if (currentAmount >= amount)
            {
                harvestedCrops[cropName] -= amount;
                return true;
            }
        }

        return false;
    }

    public Dictionary<string, int> GetAllHarvestedCrops()
    {
        return new Dictionary<string, int>(harvestedCrops);
    }
}
