using System.Collections.Generic;
using UnityEngine;

public class SwapShed : MonoBehaviour
{
    public GameObject defaultShed;
    public GameObject openShed;

    private HashSet<GameObject> playersInside = new HashSet<GameObject>();

    void Start()
    {
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
        defaultShed.SetActive(!playerInside);
    }
}