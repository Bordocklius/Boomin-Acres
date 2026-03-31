using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public partial class PlantedCrop : MonoBehaviour
{
    public CropSO CropData;
    public Transform VisualRoot;

    private float _timeSpentGrowing;
    private List<GameObject> _cropObjects;

    private CropFSM _cropFSM;

    private void Start()
    {
        InstantiateCropPool();
        _cropFSM = new CropFSM(this);
    }

    private void Update()
    {
        _cropFSM.Update(Time.deltaTime);             
    }

    private void InstantiateCropPool()
    {
        _cropObjects = new List<GameObject>(CropData.GameObjects.Count);
        foreach(GameObject obj in CropData.GameObjects)
        {
            GameObject crop = Instantiate(obj, VisualRoot);
            crop.SetActive(false);
            _cropObjects.Add(crop);
        }
    }

    public void TransitionToStage(int stage)
    {
        if(stage > 0)
            _cropObjects[stage - 1].SetActive(false);
        _cropObjects[stage].SetActive(true);
    }

    public void RequestHarvest()
    {
        _cropFSM.CurrentState.HarvestCrop();
    }

    private void Harvest()
    {
        CropManager.Instance.AddHarvestedCrop(CropData.name, CropData.CropYield);
        CropManager.Instance._maisText.text = $"Mais: {CropManager.Instance.GetHarvestedCropAmount(CropData.name)}";
    }
}
