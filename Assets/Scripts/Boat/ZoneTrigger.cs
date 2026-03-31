using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public BoatManager boatManager; // Sleep hier de boot in

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boatManager.SetPlayerInZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            boatManager.SetPlayerInZone(false);
        }
    }
}