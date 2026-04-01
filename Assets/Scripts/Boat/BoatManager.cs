using UnityEngine;

public class BoatZoneTrigger : MonoBehaviour
{
    public BoatManager boatManager;
    public bool isSellingZone; // Vink dit aan in de Inspector voor de verkoop-cirkel

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boatManager.AddPlayerToZone(isSellingZone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boatManager.RemovePlayerFromZone(isSellingZone);
        }
    }
}