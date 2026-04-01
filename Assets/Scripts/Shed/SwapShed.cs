using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SwapShed : MonoBehaviour
{
    public GameObject defaultShed;
    public GameObject openShed;

    private HashSet<GameObject> playersInside = new HashSet<GameObject>();
    private Renderer[] defaultRenderers;

    void Start()
    {
        defaultRenderers = defaultShed.GetComponentsInChildren<Renderer>(true);

        Debug.Log("Found renderers: " + defaultRenderers.Length);

        UpdateState();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside.Add(other.gameObject);
            UpdateState();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersInside.Remove(other.gameObject);
            UpdateState();
        }
    }

    void UpdateState()
    {
        bool playerInside = playersInside.Count > 0;

        openShed.SetActive(playerInside);

        foreach (var rend in defaultRenderers)
        {
            if (playerInside)
            {
                rend.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            }
            else
            {
                rend.shadowCastingMode = ShadowCastingMode.On;
            }
        }

        Debug.Log("Player inside: " + playerInside);
    }
}