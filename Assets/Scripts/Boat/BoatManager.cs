using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoatManager : MonoBehaviour
{
    [System.Serializable]
    public class CropVisuals
    {
        public string cropName;
        public GameObject cropPrefab;
    }

    [Header("Movement")]
    public Transform startPoint;
    public Transform dockPoint;
    public Transform exitPoint;
    public float speed = 5f;

    [Header("Scalable Cargo Visuals")]
    public List<CropVisuals> allCropVisuals;

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

    private enum BoatState { Coming, Waiting, Leaving, Gone }
    [SerializeField] private BoatState currentState = BoatState.Gone;

    void Start()
    {
        InitializeCropDictionary();
        StartCoroutine(BoatRoutine());
    }

    void InitializeCropDictionary()
    {
        cropPrefabDict.Clear();
        possibleCrops.Clear();

        foreach (CropVisuals visual in allCropVisuals)
        {
            if (!cropPrefabDict.ContainsKey(visual.cropName))
            {
                cropPrefabDict.Add(visual.cropName, visual.cropPrefab);
                possibleCrops.Add(visual.cropName);
            }
        }
    }

    // Aangepast: we hebben de CropManager referentie niet meer nodig als parameter
    // omdat we CropManager.Instance gebruiken.
    public void SetPlayerInZone(bool inZone)
    {
        playerInZone = inZone;
    }

    IEnumerator BoatRoutine()
    {
        while (true)
        {
            // 1. WACHTEN & RESET
            currentState = BoatState.Gone;
            transform.position = new Vector3(0, -100, 0);
            yield return new WaitForSeconds(boatRespawnTime);

            if (possibleCrops.Count == 0)
            {
                Debug.LogError("Geen crops ingevuld in BoatManager!");
                yield break;
            }

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
                // Check via de Singleton van CropManager
                if (playerInZone && CropManager.Instance != null)
                {
                    // Gebruik TryRemoveHarvestedCrop van je CropManager
                    if (CropManager.Instance.TryRemoveHarvestedCrop(currentRequiredCrop, 1))
                    {
                        SpawnFlyingItem();
                        currentFilledSlots++;
                        yield return new WaitForSeconds(timePerItem);
                    }
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
        if (cropPrefabDict.TryGetValue(currentRequiredCrop, out GameObject prefabToSpawn))
        {
            GameObject item = Instantiate(prefabToSpawn, playerBackpack.position, Quaternion.identity);
            FlyingItem flyer = item.AddComponent<FlyingItem>();
            flyer.StartFlight(playerBackpack, transform, 0.6f);
        }
    }

    void MoveBoat(Vector3 target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        Vector3 dir = (target - transform.position).normalized;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);
    }
}