using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class CropsUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _maisText;

    private void Start()
    {
        CropManager.Instance.CropHarvested += CropManager_CropHarvested;
    }

    private void OnDisable()
    {
        CropManager.Instance.CropHarvested -= CropManager_CropHarvested;
    }

    private void CropManager_CropHarvested(string cropName, int cropAmount)
    {
        cropName = cropName.ToLower();
        switch (cropName)
        {
            case "mais":
                _maisText.text = $"Mais: {cropAmount}";
                break;

            default:
                Debug.LogError("Crop not found");
                break;
        }
    }
}
