using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InitializeLevel : MonoBehaviour
{
    [SerializeField] private Transform[] _playerSpawns;
    [SerializeField] private GameObject _playerPrefab;
 
    void Start()
    {
        var playerConfigs = PlayerConfigurationManager.Instance.GetPlayerConfigs().ToArray();
        for (int i = 0; i < playerConfigs.Length; i++)
        {
            //playerConfigs[i].Input.currentActionMap
            var player = Instantiate(_playerPrefab, _playerSpawns[i].position, _playerSpawns[i].rotation, gameObject.transform);
            player.GetComponent<PlayerMovement>().InitializePlayer(playerConfigs[i]);
            player.GetComponent<FarmInteraction>().InitializePlayer(playerConfigs[i]);
            player.GetComponent<ItemHolder>().InitializePlayer(playerConfigs[i]);
            //player.gameObject.GetComponentInChildren<TMP_Text>().SetText($"Player {playerConfigs[i].PlayerId + 1}");
        }
    }
}
