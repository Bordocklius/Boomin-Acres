using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum StickDirection
{
    Neutral,
    Up,
    Down,
    Left,
    Right,
    UpRight,
    UpLeft,
    DownRight,
    DownLeft
}

public struct TimedInput
{
    public StickDirection direction;
    public float timestamp;

    public TimedInput(StickDirection direction, float timestamp)
    {
        this.direction = direction;
        this.timestamp = timestamp;
    }
}

public class InputBuffer
{
    private List<TimedInput> buffer = new List<TimedInput>();
    private int maxSize = 20;

    public void Initialize()
    {
        buffer.Clear();
        buffer.Add(new TimedInput(StickDirection.Neutral, Time.time));
    }

    public void Add(StickDirection dir)
    {
        buffer.Add(new TimedInput(dir, Time.time));
        if (buffer.Count > maxSize)
            buffer.RemoveAt(0);
    }

    public void UpdateLastTimestamp()
    {
        if (buffer.Count > 0)
        {
            var lastItem = buffer[buffer.Count - 1];
            lastItem.timestamp = Time.time;
            buffer[buffer.Count - 1] = lastItem;
        }
    }

    public void Clear()
    {
        buffer.Clear();
        buffer.Add(new TimedInput(StickDirection.Neutral, Time.time));
    }

    public TimedInput[] ToArray()
    {
        return buffer.ToArray();
    }
}

[System.Serializable]
public class InputPattern
{
    public List<StickDirection> sequence;
    public float maxTimeBetweenInputs = 0.3f;
    public UnityEngine.Events.UnityEvent onMatch;
}

public class MovementMatcher : MonoBehaviour
{
    [Header("Order from most restrictive to least restrictive\n(so uppercut goes above jab)")]
    [SerializeField]
    private List<InputPattern> patterns;
    private InputBuffer buffer;

    [SerializeField]
    private bool flipControls = false;

    private StickDirection lastDirection = StickDirection.Neutral;

    [SerializeField]
    private int armID = 0;
    private PlayerInput playerInput; //this may need to be moved to public and or have a setter, depends on how we swap from menu to game
    private InputAction moveAction;

    private void Awake()
    {
        buffer = new InputBuffer();
        buffer.Initialize();
        playerInput = GlobdataContainer.GetPlayerIDInput(armID);
    }

    private StickDirection FlipDirection(StickDirection direction)
    {
        switch (direction)
        {
            case StickDirection.Left:
                return StickDirection.Right;
            case StickDirection.Right:
                return StickDirection.Left;
            case StickDirection.UpLeft:
                return StickDirection.UpRight;
            case StickDirection.UpRight:
                return StickDirection.UpLeft;
            case StickDirection.DownLeft:
                return StickDirection.DownRight;
            case StickDirection.DownRight:
                return StickDirection.DownLeft;
            default:
                return direction;
        }
    }

    private void OnEnable()
    {
        if (playerInput == null)
            playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Look"];
        moveAction.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null)
            moveAction.Disable();

        buffer.Clear();
        lastDirection = StickDirection.Neutral;
    }

    private StickDirection GetDirection(Vector2 input, float deadzone = 0.5f)
    {
        if (input.magnitude < deadzone)
            return StickDirection.Neutral;

        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;

        if (angle < 0) angle += 360;

        if (angle >= 325f || angle < 35f) return StickDirection.Right;
        if (angle < 55f) return StickDirection.UpRight;
        if (angle < 125f) return StickDirection.Up;
        if (angle < 145f) return StickDirection.UpLeft;
        if (angle < 215f) return StickDirection.Left;
        if (angle < 235f) return StickDirection.DownLeft;
        if (angle < 305f) return StickDirection.Down;
        return StickDirection.DownRight;
    }

    bool MatchPattern(TimedInput[] buffer, InputPattern pattern)
    {
        if (pattern == null || pattern.sequence == null || pattern.sequence.Count == 0 || buffer.Length < pattern.sequence.Count)
        { 
            return false; 
        }

        int bufferStartIndex = buffer.Length - pattern.sequence.Count;

        for (int i = 0; i < pattern.sequence.Count; i++)
        {
            if (buffer[bufferStartIndex + i].direction != pattern.sequence[i])
            {
                return false;
            }

            if (i > 0)
            {
                float timeDifference = buffer[bufferStartIndex + i].timestamp - buffer[bufferStartIndex + i - 1].timestamp;
                if (timeDifference > pattern.maxTimeBetweenInputs)
                    return false;
            }
        }

        return true;
    }

    private void Update()
    {
        if (moveAction == null || patterns == null)
            return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        StickDirection dir = GetDirection(input);
        if (flipControls)
        {
            dir = FlipDirection(dir);
        }

        //only add new directions - they get timestamped anyways
        if (dir != lastDirection)
        {
            buffer.Add(dir);
            lastDirection = dir;

            foreach (var pattern in patterns)
            {
                if (pattern != null && MatchPattern(buffer.ToArray(), pattern))
                {
                    pattern.onMatch.Invoke();
                    break; //pattern found = exit early
                }
            }
        }
        else
        {
            buffer.UpdateLastTimestamp();
        }
    }
}
