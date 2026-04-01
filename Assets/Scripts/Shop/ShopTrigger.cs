using UnityEngine;
using UnityEngine.InputSystem; // Belangrijk voor controller input

public class ShopTrigger : MonoBehaviour
{
    [Header("Shop Settings")]
    public CropSO cropToSell;
    public int price = 10;
    public GameObject visualIndicator; // Bijv. een zwevend icoontje boven de cirkel

    private bool playerInRange = false;

    void Start()
    {
        if (visualIndicator != null) visualIndicator.SetActive(false);
    }

    void Update()
    {
        // Check of de speler in de zone staat en op de 'West Button' drukt
        // 'buttonWest' is X op Xbox / Square op PlayStation
        if (playerInRange && GameplayInputCheck())
        {
            BuySeed();
        }
    }

    bool GameplayInputCheck()
    {
        // Gebruik je de nieuwe Input System? Dan is dit de check:
        if (Gamepad.current != null && Gamepad.current.buttonWest.wasPressedThisFrame) return true;

        // Optioneel: ook een toetsenbord toets (bijv. F)
        if (Keyboard.current.fKey.wasPressedThisFrame) return true;

        return false;
    }

    void BuySeed()
    {
        if (ShopManager.Instance.TryBuyItem(price))
        {
            
            CropManager.Instance.AddHarvestedCrop(cropToSell.CropName, 1);

            Debug.Log($"Je hebt {cropToSell.CropName} zaad gekocht!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (visualIndicator != null) visualIndicator.SetActive(true);
            Debug.Log($"Stap in de zone: {cropToSell.CropName} kost {price} goud. Druk op West-button!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (visualIndicator != null) visualIndicator.SetActive(false);
        }
    }
}