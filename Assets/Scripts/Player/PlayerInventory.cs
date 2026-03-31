using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int grainAmount = 5; // Start met 5 graan

    public bool HasGrain()
    {
        return grainAmount > 0;
    }

    public void RemoveGrain()
    {
        grainAmount--;
        Debug.Log("Graan uitgegeven! Resterend: " + grainAmount);
    }
}