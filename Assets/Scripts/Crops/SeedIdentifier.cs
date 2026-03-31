using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class SeedIdentifier : MonoBehaviour
{
    [SerializeField] private string cropName;

    public string CropName => cropName;
}

