using UnityEngine;
using System.Collections;

public class BoatManager : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform startPoint;
    public Transform dockPoint;
    public Transform exitPoint;
    public float speed = 5f;

    [Header("Filling & Inventory")]
    public int totalSlots = 6;
    public float timePerItem = 0.8f;
    public GameObject grainPrefab;
    public Transform playerBackpack;

    [Header("Timing Settings")]
    [Tooltip("Hoeveel seconden het duurt voordat er een nieuwe boot komt.")]
    public float boatRespawnTime = 20f;

    private int currentFilledSlots = 0;
    private bool playerInZone = false;
    private PlayerInventory playerInv;

    private enum BoatState { Coming, Waiting, Leaving, Gone }
    [SerializeField] private BoatState currentState = BoatState.Gone;

    void Start()
    {
        // We zorgen dat de boot onzichtbaar is of ver weg staat bij het opstarten
        transform.position = new Vector3(0, -100, 0);
        StartCoroutine(BoatRoutine());
    }

    public void SetPlayerInZone(bool inZone, PlayerInventory inventory = null)
    {
        playerInZone = inZone;
        playerInv = inventory;
    }

    IEnumerator BoatRoutine()
    {
        while (true)
        {
            // --- STAP 1: WACHTEN OP RESPAWN ---
            currentState = BoatState.Gone;
            Debug.Log($"Wachten op nieuwe boot... ({boatRespawnTime} seconden)");
            yield return new WaitForSeconds(boatRespawnTime);

            // --- STAP 2: SPAWN BIJ STARTPOSITION ---
            // De boot "teleporteert" direct naar het startpunt
            transform.position = startPoint.position;
            currentFilledSlots = 0;
            currentState = BoatState.Coming;
            Debug.Log("Boot gespawned op startpunt!");

            // --- STAP 3: VAAR NAAR DOK ---
            while (Vector3.Distance(transform.position, dockPoint.position) > 0.5f)
            {
                MoveBoat(dockPoint.position);
                yield return null;
            }

            transform.position = dockPoint.position;
            currentState = BoatState.Waiting;

            // --- STAP 4: VULLEN ---
            while (currentFilledSlots < totalSlots)
            {
                if (playerInZone && playerInv != null && playerInv.HasGrain())
                {
                    SpawnFlyingGrain();
                    playerInv.RemoveGrain();
                    currentFilledSlots++;
                    yield return new WaitForSeconds(timePerItem);
                }
                yield return null;
            }

            // --- STAP 5: VERTREKKEN ---
            yield return new WaitForSeconds(1f);
            currentState = BoatState.Leaving;

            while (Vector3.Distance(transform.position, exitPoint.position) > 0.5f)
            {
                MoveBoat(exitPoint.position);
                yield return null;
            }

            // Boot "verdwijnt" weer onder de grond/uit beeld terwijl de timer loopt
            transform.position = new Vector3(0, -100, 0);
        }
    }

    void SpawnFlyingGrain()
    {
        GameObject grain = Instantiate(grainPrefab, playerBackpack.position, Quaternion.identity);
        FlyingItem flyer = grain.AddComponent<FlyingItem>();
        flyer.StartFlight(playerBackpack, transform, 0.6f);
    }

    void MoveBoat(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);
    }
}