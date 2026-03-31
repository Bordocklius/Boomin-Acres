using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Crop", menuName = "Crops/Crop")]
public class CropSO : ScriptableObject
{
    public string CropName;
    public List<GameObject> GameObjects;
    public float GrowthTime;
    public Sprite CropSprite;
}

public enum CropStatus
{
    None,
    Planted,
    NotGrowing,
    Growing,
    Grown
}
