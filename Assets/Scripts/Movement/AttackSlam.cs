using UnityEngine;

public class AttackSlam : Attack
{
    [SerializeField]
    private GameObject slamUpDest;   // arms fold up to here first
    [SerializeField]
    private GameObject slamDownDest; // then slam down to here
    [SerializeField]
    private float slamSpeed;

    public override void OnAttack()
    {
        attachedMovementHandler.enabled = false;
        StartCoroutine(SlamCoroutine());
    }

    private System.Collections.IEnumerator SlamCoroutine()
    {
        Vector3 startPos = handObject.transform.position;
        Vector3 upPos = slamUpDest.transform.position;
        Vector3 downPos = slamDownDest.transform.position;

        // Phase 1: fold arm up
        float totalTime = Vector3.Distance(startPos, upPos) / slamSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(startPos, upPos, t);
            yield return null;
        }
        handObject.transform.position = upPos;

        // Phase 2: slam arm down hard
        totalTime = Vector3.Distance(upPos, downPos) / slamSpeed;
        elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(upPos, downPos, t);
            yield return null;
        }
        handObject.transform.position = downPos;

        // Phase 3: return to start
        totalTime = Vector3.Distance(downPos, startPos) / slamSpeed;
        elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(downPos, startPos, t);
            yield return null;
        }
        handObject.transform.position = startPos;

        OnAttackeEnd();
    }

    protected override void OnAttackeEnd()
    {
        attachedMovementHandler.enabled = true;
    }
}
