using UnityEngine;
using System.Collections;

public class BoatManager : MonoBehaviour
{
    [Header("Movement")]
    public Transform startPoint;
    public Transform dockPoint;
    public Transform exitPoint;
    public float speed = 5f;

    [Header("Filling Logic")]
    public int totalSlots = 6;
    public float timePerItem = 1.0f; // Snelheid van vullen
    [SerializeField] private int currentFilledSlots = 0;

    private bool playerInZone = false;
    private enum BoatState { Coming, Waiting, Leaving, Gone }
    private BoatState currentState = BoatState.Gone;

    void Start()
    {
        StartCoroutine(BoatRoutine());
    }

    // Deze functies worden aangeroepen als de speler de cirkel op de pier raakt
    // Zorg dat de cirkel de tag "FillZone" heeft of dit script op de cirkel staat
    public void SetPlayerInZone(bool inZone)
    {
        playerInZone = inZone;
        if (inZone) Debug.Log("Laden gestart...");
        else Debug.Log("Laden gestopt: Speler is uit de cirkel.");
    }

    IEnumerator BoatRoutine()
    {
        while (true)
        {
            // 1. SPAWN & VAAR NAAR PIER
            transform.position = startPoint.position;
            currentFilledSlots = 0;
            currentState = BoatState.Coming;

            while (Vector3.Distance(transform.position, dockPoint.position) > 0.1f)
            {
                MoveBoat(dockPoint.position);
                yield return null;
            }

            // 2. WACHTEN BIJ PIER
            currentState = BoatState.Waiting;
            Debug.Log("Boot ligt klaar. Stap in de cirkel!");

            while (currentFilledSlots < totalSlots)
            {
                if (playerInZone)
                {
                    yield return new WaitForSeconds(timePerItem);
                    currentFilledSlots++;
                    Debug.Log($"Item {currentFilledSlots}/{totalSlots} geladen!");
                }
                yield return null;
            }

            // 3. VERTREKKEN
            Debug.Log("Boot is vol! Tot ziens.");
            currentState = BoatState.Leaving;

            while (Vector3.Distance(transform.position, exitPoint.position) > 0.1f)
            {
                MoveBoat(exitPoint.position);
                yield return null;
            }

            // 4. DESPAWN & COOLDOWN
            currentState = BoatState.Gone;
            transform.position = new Vector3(0, -100, 0);
            yield return new WaitForSeconds(20f);
        }
    }

    void MoveBoat(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);
    }
}