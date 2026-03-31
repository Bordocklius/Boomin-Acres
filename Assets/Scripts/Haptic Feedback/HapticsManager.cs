using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class HapticsManager : MonoBehaviour
{
    // other script calls Play(...) -> we find this player's gamepad -> start vibration -> wait -> stop vibration

    [SerializeField] private PlayerInput playerInput;

    [SerializeField, Range(0f, 1f)] private float defaultLowFrequency = 0.25f;
    [SerializeField, Range(0f, 1f)] private float defaultHighFrequency = 0.25f;

    private Coroutine hapticsRoutine;
    private Gamepad cachedGamepad;

    public void Play(float durationSeconds)
    {
        Play(durationSeconds, defaultLowFrequency, defaultHighFrequency);
    }

    public void Play(float durationSeconds, float lowFrequency, float highFrequency)
    {
        Gamepad gamepad = GetGamepad();

        if (gamepad == null)
        {
            return;
        }

        Stop();

        if (durationSeconds <= 0f)
        {
            return;
        }

        float clampedLow = Mathf.Clamp01(lowFrequency);
        float clampedHigh = Mathf.Clamp01(highFrequency);

        hapticsRoutine = StartCoroutine(PlayRoutine(gamepad, durationSeconds, clampedLow, clampedHigh));
    }

    public void Stop()
    {
        if (hapticsRoutine != null)
        {
            StopCoroutine(hapticsRoutine);
            hapticsRoutine = null;
        }

        if (cachedGamepad != null)
        {
            cachedGamepad.ResetHaptics();
        }
    }

    private void OnDisable()
    {
        Stop();
    }

    private IEnumerator PlayRoutine(Gamepad gamepad, float durationSeconds, float lowFrequency, float highFrequency)
    {
        gamepad.SetMotorSpeeds(lowFrequency, highFrequency);

        yield return new WaitForSeconds(durationSeconds);

        gamepad.ResetHaptics();
        hapticsRoutine = null;
    }

    private Gamepad GetGamepad()
    {
        if (cachedGamepad != null)
        {
            return cachedGamepad;
        }

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (playerInput == null)
        {
            return null;
        }

        ReadOnlyArray<InputDevice> devices = playerInput.devices;

        for (int idx = 0; idx < devices.Count; idx++)
        {
            Gamepad gamepad = devices[idx] as Gamepad;

            if (gamepad != null)
            {
                cachedGamepad = gamepad;
                return cachedGamepad;
            }
        }

        return null;
    }
}