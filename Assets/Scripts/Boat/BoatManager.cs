using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class BoatManager : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform startPoint;
    public Transform dockPoint;
    public Transform exitPoint;

    [Header("Settings")]
    public float speed = 5f;
    public float respawnDelay = 5f;
    public float interactionDistance = 3f; // Hoe dichtbij moet de speler zijn?

    [Header("UI Reference")]
    public GameObject boatMenuPanel; // Sleep hier je Panel in
    public Transform playerTransform; // Sleep hier je Player in

    private enum BoatState { Coming, Waiting, Leaving, Gone }
    [SerializeField] private BoatState currentState = BoatState.Gone;
    private bool goodsAreFilled = false;

    void Start()
    {
        boatMenuPanel.SetActive(false);
        StartCoroutine(BoatRoutine());
    }

    void Update()
    {
        // We checken alleen voor input als de boot bij het dok ligt
        if (currentState == BoatState.Waiting)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance <= interactionDistance && Keyboard.current.eKey.wasPressedThisFrame)
            {
                // Als het menu al open is, sluit het. Anders, open het.
                if (boatMenuPanel.activeSelf)
                {
                    CloseBoatMenu();
                }
                else
                {
                    OpenBoatMenu();
                }
            }
        }
    }

    void OpenBoatMenu()
    {
        boatMenuPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseBoatMenu()
    {
        boatMenuPanel.SetActive(false);
        // Zet de cursor weer vast voor de speler
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Deze functie blijft voor je UI Button
    public void FillGoodsAndSend()
    {
        goodsAreFilled = true;
        CloseBoatMenu(); // Gebruik de nieuwe sluit-functie
        currentState = BoatState.Leaving;
    }

    IEnumerator BoatRoutine()
    {
        while (true)
        {
            // 1. SPAWN
            transform.position = startPoint.position;
            goodsAreFilled = false;
            currentState = BoatState.Coming;

            // 2. VAAR NAAR DOK
            while (Vector3.Distance(transform.position, dockPoint.position) > 0.2f)
            {
                MoveBoat(dockPoint.position);
                yield return null;
            }

            transform.position = dockPoint.position;
            currentState = BoatState.Waiting;

            // 3. WACHT TOT DE STATE VERANDERT (door de UI knop)
            while (currentState == BoatState.Waiting)
            {
                yield return null;
            }

            // 4. VERTREKKEN
            while (Vector3.Distance(transform.position, exitPoint.position) > 0.2f)
            {
                MoveBoat(exitPoint.position);
                yield return null;
            }

            // 5. DESPAWN
            currentState = BoatState.Gone;
            transform.position = new Vector3(0, -50, 0);

            yield return new WaitForSeconds(respawnDelay);
        }
    }

    void MoveBoat(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2f);
        }
    }
}