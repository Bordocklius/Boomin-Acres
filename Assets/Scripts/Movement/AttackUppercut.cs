using UnityEngine;

public class AttackUppercut : Attack
{
    [SerializeField]
    private GameObject uppercutBottomDest; // arm pulls down to here first
    [SerializeField]
    private GameObject uppercutTopDest;    // then sweeps up to here
    [SerializeField]
    private float uppercutSpeed;

    public override void OnAttack()
    {
        attachedMovementHandler.enabled = false;
        StartCoroutine(UppercutCoroutine());
    }

    private System.Collections.IEnumerator UppercutCoroutine()
    {
        Vector3 startPos = handObject.transform.position;
        Vector3 bottomPos = uppercutBottomDest.transform.position;
        Vector3 topPos = uppercutTopDest.transform.position;

        // Phase 1: pull arm down
        float totalTime = Vector3.Distance(startPos, bottomPos) / uppercutSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(startPos, bottomPos, t);
            yield return null;
        }
        handObject.transform.position = bottomPos;

        // Phase 2: sweep arm up
        totalTime = Vector3.Distance(bottomPos, topPos) / uppercutSpeed;
        elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(bottomPos, topPos, t);
            yield return null;
        }
        handObject.transform.position = topPos;

        // Phase 3: return to start
        totalTime = Vector3.Distance(topPos, startPos) / uppercutSpeed;
        elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(topPos, startPos, t);
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
