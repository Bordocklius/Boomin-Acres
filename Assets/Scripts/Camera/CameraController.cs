using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    [Header("Positioning")]
    [Tooltip("De vaste afstand en hoek ten opzichte van de spelers")]
    public Vector3 cameraOffset = new Vector3(0, 10, -10);
    public float smoothSpeed = 5f;

    [Header("Obstacle Avoidance")]
    public LayerMask obstacleMask;
    public float checkRadius = 0.5f;

    private List<PlayerMovement> _players = new List<PlayerMovement>();

    void Start()
    {
        StartCoroutine(FindPlayers());
    }

    IEnumerator FindPlayers()
    {
        // Zoekt naar minimaal 2 spelers (pas aan naar 1 als je alleen speelt)
        while (_players.Count < 2)
        {
            _players.Clear();
            var foundPlayers = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
            foreach (var p in foundPlayers)
                _players.Add(p);
            yield return null;
        }
    }

    void LateUpdate()
    {
        if (_players.Count == 0) return;

        // 1. Bereken het gemiddelde middelpunt van alle spelers
        Vector3 playersCenter = GetPlayersCenter();

        // 2. Bepaal de gewenste positie (Middelpunt + de vaste offset)
        Vector3 desiredPosition = playersCenter + cameraOffset;

        // 3. Obstacle avoidance (Raycast van spelers naar camera)
        float maxDist = cameraOffset.magnitude;
        Vector3 dirToCamera = (desiredPosition - playersCenter).normalized;

        if (Physics.SphereCast(playersCenter, checkRadius, dirToCamera, out RaycastHit hit, maxDist, obstacleMask))
        {
            // Zet de camera dichterbij als er iets tussen zit
            desiredPosition = hit.point - dirToCamera * 0.2f;
        }

        // 4. Beweeg de camera soepel naar de positie
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

        // 5. Laat de camera altijd naar het midden van de spelers kijken
        transform.LookAt(playersCenter);
    }

    Vector3 GetPlayersCenter()
    {
        Vector3 sum = Vector3.zero;
        int count = 0;
        foreach (var p in _players)
        {
            if (p != null)
            {
                sum += p.transform.position;
                count++;
            }
        }
        return count > 0 ? sum / count : Vector3.zero;
    }
}