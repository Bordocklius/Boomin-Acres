using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public BoatManager boatManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inv = other.GetComponent<PlayerInventory>();
            boatManager.SetPlayerInZone(true, inv);
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