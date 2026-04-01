using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    public BoatManager boatManager;

    private void OnTriggerEnter(Collider other)
    {        
        if (other.CompareTag("Player"))
        {            
            boatManager.SetPlayerInZone(true);
            boatManager.PlayersInZone++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check of de speler de cirkel verlaat
        if (other.CompareTag("Player"))
        {
            boatManager.SetPlayerInZone(false);
            boatManager.PlayersInZone--;
        }
    }
}