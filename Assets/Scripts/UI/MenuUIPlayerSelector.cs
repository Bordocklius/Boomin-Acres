using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class MenuUIPlayerSelector : MonoBehaviour
{
    [SerializeField]
    private int playerSlot;
    TMP_Text playerNumberText;
    private PlayerInput claimedPlayer = null;
    private Dictionary<PlayerInput, Action<InputAction.CallbackContext>> claimCallbacks = new Dictionary<PlayerInput, Action<InputAction.CallbackContext>>();
    private Dictionary<PlayerInput, Action<InputAction.CallbackContext>> unclaimCallbacks = new Dictionary<PlayerInput, Action<InputAction.CallbackContext>>();
    private Dictionary<PlayerInput, Vector3> playerPositions = new Dictionary<PlayerInput, Vector3>();

    [SerializeField]
    private UnityEvent onClaim;
    [SerializeField]
    private UnityEvent onUnclaim;


    private void Start()
    {
        playerNumberText = GetComponentInChildren<TMP_Text>();  
        playerNumberText.text = $"";
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        RemoveAllCallbacks();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        RemoveAllCallbacks();
    }

    private void RemoveAllCallbacks()
    {
        foreach (var playerInput in new List<PlayerInput>(claimCallbacks.Keys))
        {
            if (playerInput != null)
            {
                playerInput.actions["Submit"].performed -= claimCallbacks[playerInput];
            }
        }
        claimCallbacks.Clear();

        foreach (var playerInput in new List<PlayerInput>(unclaimCallbacks.Keys))
        {
            if (playerInput != null)
            {
                playerInput.actions["Cancel"].performed -= unclaimCallbacks[playerInput];
            }
        }
        unclaimCallbacks.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var playerInput = other.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            Debug.Log($"Player {playerInput.playerIndex} entered square");
            var claimCallback = ClaimSlot(playerInput);
            claimCallbacks[playerInput] = claimCallback;
            playerInput.actions["Submit"].performed += claimCallback;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var playerInput = other.GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            Debug.Log($"Player {playerInput.playerIndex} left square");

            if (claimCallbacks.TryGetValue(playerInput, out var claimCallback))
            {
                playerInput.actions["Submit"].performed -= claimCallback;
                claimCallbacks.Remove(playerInput);
            }

            //if this is the claimed player, keep the unclaim callback active
            //they should still be able to unclaim even though they've left the trigger zone
            if (claimedPlayer != playerInput)
            {

                if (unclaimCallbacks.TryGetValue(playerInput, out var unclaimCallback))
                {
                    playerInput.actions["Cancel"].performed -= unclaimCallback;
                    unclaimCallbacks.Remove(playerInput);
                }
            }
        }
    }

    private void SetPlayerVisibility(PlayerInput playerInput, bool isVisible)
    {
        var image = playerInput.GetComponent<Image>();
        if (image != null)
        {
            image.enabled = isVisible;
        }
        var tmpText = playerInput.GetComponentInChildren<TextMeshProUGUI>();
        if (tmpText != null)
        {
            tmpText.enabled = isVisible;
        }
        var menuController = playerInput.GetComponent<MenuUIController>();
        if (menuController != null)
        {
            menuController.enabled = isVisible;
        }
    }

    private Action<InputAction.CallbackContext> UnclaimSlot(PlayerInput playerInput)
    {
        return (ctx) =>
        {
            if (claimedPlayer == playerInput && playerInput != null && playerInput.gameObject != null)
            {
                GlobdataContainer.UnregisterInputGlobal(playerSlot);
                GlobdataContainer.ReleaseObjectFromGlobal(playerInput.gameObject);

                claimedPlayer = null;
                SetPlayerVisibility(playerInput, true);
                if (unclaimCallbacks.TryGetValue(playerInput, out var callback))
                {
                    playerInput.actions["Cancel"].performed -= callback;
                    unclaimCallbacks.Remove(playerInput);
                }

                playerInput.gameObject.AddComponent<MenUIReparenter>();

                //restore world position next frame after reparenter does its job
                StartCoroutine(RestorePositionNextFrame(playerInput));

                onUnclaim.Invoke();

                playerNumberText.text = $"";
            }
        };
    }

    private Action<InputAction.CallbackContext> ClaimSlot(PlayerInput playerInput)
    {        
        return (ctx) =>
        {
            if (claimedPlayer == null && playerInput != null && playerInput.gameObject != null)
            {
                //store world position before claiming
                playerPositions[playerInput] = playerInput.gameObject.transform.position;

                claimedPlayer = playerInput;
                if (claimCallbacks.TryGetValue(playerInput, out var claimCallback))
                {
                    playerInput.actions["Submit"].performed -= claimCallback;
                }
                SetPlayerVisibility(playerInput, false);
                var unclaimCallback = UnclaimSlot(playerInput);
                unclaimCallbacks[playerInput] = unclaimCallback;
                playerInput.actions["Cancel"].performed += unclaimCallback;
                playerNumberText.text = $"Player {playerInput.playerIndex + 1}";

                GlobdataContainer.RegisterInputGlobal(playerInput.gameObject, playerSlot);

                playerInput.gameObject.GetComponent<HapticsManager>()?.Play(0.5f);

                onClaim.Invoke();
            }
        };
    }

    private IEnumerator RestorePositionNextFrame(PlayerInput playerInput)
    {
        yield return null;
        if (playerPositions.TryGetValue(playerInput, out var storedPosition))
        {
            playerInput.gameObject.transform.position = storedPosition;
            playerPositions.Remove(playerInput);
        }
    }
}
