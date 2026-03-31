using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatManager : MonoBehaviour
{
    public Transform startPoint;  // Waar de boot begint
    public Transform dockPoint;   // Waar de boot stopt voor de speler
    public Transform exitPoint;   // Waar de boot naartoe vaart na vertrek

    public float speed = 5f;      // Hoe snel de boot vaart
    public float respawnDelay = 20f; // Wachttijd voor een nieuwe boot

    private enum BoatState { Coming, Waiting, Leaving, Gone }
    private BoatState currentState = BoatState.Gone;

    void Start()
    {
        // Start de cyclus direct
        StartCoroutine(BoatRoutine());
    }

    void Update()
    {
        // Check of de 'E' toets is ingedrukt met het nieuwe Input System
        if (currentState == BoatState.Waiting && Keyboard.current.eKey.wasPressedThisFrame)
        {
            currentState = BoatState.Leaving;
        }
    }

    IEnumerator BoatRoutine()
    {
        while (true)
        {
            // 1. SPAWN & AANKOMEN
            transform.position = startPoint.position;
            currentState = BoatState.Coming;
            Debug.Log("Boot komt eraan...");

            while (Vector3.Distance(transform.position, dockPoint.position) > 0.1f)
            {
                MoveBoat(dockPoint.position);
                yield return null;
            }

            // 2. WACHTEN OP SPELER
            currentState = BoatState.Waiting;
            Debug.Log("Boot ligt aan de kade. Druk op E om te vertrekken.");

            // We blijven hier hangen zolang de state 'Waiting' is
            while (currentState == BoatState.Waiting)
            {
                yield return null;
            }

            // 3. VERTREKKEN
            Debug.Log("Boot vertrekt!");
            while (Vector3.Distance(transform.position, exitPoint.position) > 0.1f)
            {
                MoveBoat(exitPoint.position);
                yield return null;
            }

            // 4. WEGWEZEN & WACOUPON
            currentState = BoatState.Gone;
            Debug.Log($"Boot is weg. Volgende boot over {respawnDelay} seconden.");

            // Verplaats de boot even ver weg (of zet mesh renderer uit)
            transform.position = new Vector3(0, -100, 0);

            yield return new WaitForSeconds(respawnDelay);
        }
    }

    void MoveBoat(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Zorg dat de boot ook de goede kant op kijkt (rotatie)
        Vector3 direction = target - transform.position;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2f);
        }
    }
}