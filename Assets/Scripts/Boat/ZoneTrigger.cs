using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public BoatManager boatManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boatManager.AddPlayerToZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boatManager.RemovePlayerFromZone();
        }
    }
}