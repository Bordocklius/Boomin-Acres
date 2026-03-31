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

    private CropFSM _cropFSM;

    private void Awake()
    {
        _cropFSM = new CropFSM(this);
    }

    private void Update()
    {
        _cropFSM.Update(Time.deltaTime);
    }

    private void DestroyVisualRoot()
    {
        foreach (Transform child in VisualRoot.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void TransitionToStage(int stage)
    {
        DestroyVisualRoot();
        Instantiate(CropData.GameObjects[stage], VisualRoot);
    }
}
