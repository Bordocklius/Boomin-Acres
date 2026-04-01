using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTrigger : MonoBehaviour
{
    [Header("Shop Settings")]
    public CropSO cropToBuy;
    public int price = 10;
    public GameObject visualIndicator;

    [Header("Spawn Settings")]
    public GameObject seedPrefab;      // Het 3D model van het zaadzakje
    public Transform spawnLocation;    // Sleep hier je 'Empty GameObject' in

    [Header("Input Action")]
    public InputActionReference interactAction;

    private bool playerInRange = false;

    private void OnEnable() => interactAction.action.Enable();
    private void OnDisable() => interactAction.action.Disable();

    void Update()
    {
        if (playerInRange && interactAction.action.WasPressedThisFrame())
        {
            ProcessPurchase();
        }
    }

    void ProcessPurchase()
    {
        if (ShopManager.Instance != null && ShopManager.Instance.TryBuyItem(price))
        {
            // 1. Voeg toe aan de digitale lijst
            CropManager.Instance.AddHarvestedCrop(cropToBuy.CropName, 1);

            // 2. SPAWN het fysieke object in de wereld
            SpawnSeedPhysical();

            Debug.Log($"Gekocht en gespawned: {cropToBuy.CropName}!");
        }
    }

    void SpawnSeedPhysical()
    {
        if (seedPrefab != null && spawnLocation != null)
        {
            // Maak het zaadje aan op de plek van het Empty GameObject
            GameObject newSeed = Instantiate(seedPrefab, spawnLocation.position, spawnLocation.rotation);

            // Optioneel: Geef het een kleine "hop" omhoog met physics als het een Rigidbody heeft
            Rigidbody rb = newSeed.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
            }
        }
    }

    // --- OnTrigger functies blijven hetzelfde als voorheen ---
    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) { playerInRange = true; if (visualIndicator != null) visualIndicator.SetActive(true); } }
    private void OnTriggerExit(Collider other) { if (other.CompareTag("Player")) { playerInRange = false; if (visualIndicator != null) visualIndicator.SetActive(false); } }
}