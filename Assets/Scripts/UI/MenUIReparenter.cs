using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenUIReparenter : MonoBehaviour
{
    PlayerInput myInput;
    [SerializeField]
    private TMP_Text playerNumberText;

    void Start()
    {
        var GO = GameObject.Find("UI_PlayerSelect");
        if(GO != null)
        {
            this.transform.SetParent(GO.transform, false);
            this.transform.localScale = Vector3.one;
            myInput = GetComponent<PlayerInput>();
            if(playerNumberText != null)
            {
                playerNumberText.text = $"{myInput.playerIndex + 1}";
            }
        }
        Destroy(this);
    }
}
