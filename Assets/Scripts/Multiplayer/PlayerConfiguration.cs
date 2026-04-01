using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfiguration
{
    public PlayerInput Input { get; set; }
    public int PlayerId { get; set; }
    public bool IsReady { get; set; }
    public Material PlayerMaterial { get; set; }

    public PlayerConfiguration(PlayerInput input)
    {
        PlayerId = input.playerIndex;
        Input = input;
    }
}
