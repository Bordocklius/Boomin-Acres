using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoatManager : MonoBehaviour
{
    // Een simpele klasse om de naam en de prefab te koppelen
    [System.Serializable]
    public class CropVisuals
    {
        public string cropName; // Bijv: "Wortel"
        public GameObject cropPrefab; // Sleep hier PFB_Wortel in
    }

    [Header("Movement")]
    public Transform startPoint; public Transform dockPoint; public Transform exitPoint;
    public float speed = 5f;

    [Header("Scalable Cargo Visuals")]
    // Deze lijst vul je in de Inspector
    public List<CropVisuals> allCropVisuals;

    // Een Dictionary om snel de juiste prefab te vinden op basis van de naam
    private Dictionary<string, GameObject> cropPrefabDict = new Dictionary<string, GameObject>();
    private List<string> possibleCrops = new List<string>();

    [Header("Settings")]
    public Transform playerBackpack;
    public int totalSlots = 6;
    public float timePerItem = 0.8f;
    public float boatRespawnTime = 10f;

    private string currentRequiredCrop;
    private int currentFilledSlots = 0;
    private bool playerInZone = false;
    private PlayerInventory playerInv;

    private enum BoatState { Coming, Waiting, Leaving, Gone }
    [SerializeField] private BoatState currentState = BoatState.Gone;

    void Start()
    {
        InitializeCropDictionary();
        StartCoroutine(BoatRoutine());
    }

    // Zet de lijst uit de Inspector om in een snelle Dictionary
    void InitializeCropDictionary()
    {
        cropPrefabDict.Clear();
        possibleCrops.Clear();

        foreach (CropVisuals visual in allCropVisuals)
        {
            if (!cropPrefabDict.ContainsKey(visual.cropName))
            {
                cropPrefabDict.Add(visual.cropName, visual.cropPrefab);
                possibleCrops.Add(visual.cropName); // De boot kiest alleen uit ingevulde crops
            }
        }
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
            // 1. WACHTEN & RESET
            currentState = BoatState.Gone;
            transform.position = new Vector3(0, -100, 0);
            yield return new WaitForSeconds(boatRespawnTime);

            // Controleer of we wel crops hebben ingesteld
            if (possibleCrops.Count == 0)
            {
                Debug.LogError("Oeps! Je hebt geen crops ingevuld in de BoatManager Inspector.");
                yield break;
            }

            // Kies een willekeurig gewas voor deze lading
            currentRequiredCrop = possibleCrops[Random.Range(0, possibleCrops.Count)];
            currentFilledSlots = 0;
            Debug.Log($"NIEUWE BOOT: Wil graag {totalSlots}x {currentRequiredCrop} hebben!");

            // 2. SPAWN & VAAR NAAR DOK
            transform.position = startPoint.position;
            currentState = BoatState.Coming;
            while (Vector3.Distance(transform.position, dockPoint.position) > 0.5f)
            {
                MoveBoat(dockPoint.position);
                yield return null;
            }

            // 3. VULLEN
            currentState = BoatState.Waiting;
            while (currentFilledSlots < totalSlots)
            {
                if (playerInZone && playerInv != null && playerInv.HasItem(currentRequiredCrop))
                {
                    SpawnFlyingItem(); // Deze functie spawnt nu het juiste model
                    playerInv.RemoveItem(currentRequiredCrop);
                    currentFilledSlots++;
                    yield return new WaitForSeconds(timePerItem);
                }
                yield return null;
            }

            // 4. VERTREK
            yield return new WaitForSeconds(1f);
            currentState = BoatState.Leaving;
            while (Vector3.Distance(transform.position, exitPoint.position) > 0.5f)
            {
                MoveBoat(exitPoint.position);
                yield return null;
            }
        }
    }

    void SpawnFlyingItem()
    {
        // Zoek de juiste prefab in de Dictionary op basis van de gevraagde crop
        if (cropPrefabDict.TryGetValue(currentRequiredCrop, out GameObject prefabToSpawn))
        {
            // Spawn het specifieke model (wortel, graan, etc.)
            GameObject item = Instantiate(prefabToSpawn, playerBackpack.position, Quaternion.identity);

            // Plak het vlieg-script erop
            FlyingItem flyer = item.AddComponent<FlyingItem>();
            flyer.StartFlight(playerBackpack, transform, 0.6f);
        }
        else
        {
            Debug.LogError($"Geen prefab gevonden voor crop: {currentRequiredCrop}. Check je Inspector!");
        }
    }

    void MoveBoat(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero) transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);
    }
}