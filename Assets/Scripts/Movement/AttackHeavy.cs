using UnityEngine;

public class AttackHeavy : Attack
{
    [SerializeField]
    private GameObject heavyBackDest;    // arm pulls back to here first
    [SerializeField]
    private GameObject heavyForwardDest; // then drives forward to here
    [SerializeField]
    private float heavySpeed;

    public override void OnAttack()
    {
        attachedMovementHandler.enabled = false;
        StartCoroutine(HeavyCoroutine());
    }

    private System.Collections.IEnumerator HeavyCoroutine()
    {
        Vector3 startPos = handObject.transform.position;
        Vector3 backPos = heavyBackDest.transform.position;
        Vector3 forwardPos = heavyForwardDest.transform.position;

        // Phase 1: pull arm back
        float totalTime = Vector3.Distance(startPos, backPos) / heavySpeed;
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(startPos, backPos, t);
            yield return null;
        }
        handObject.transform.position = backPos;

        // Phase 2: drive arm forward fast
        totalTime = Vector3.Distance(backPos, forwardPos) / heavySpeed;
        elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(backPos, forwardPos, t);
            yield return null;
        }
        handObject.transform.position = forwardPos;

        // Phase 3: return to start
        totalTime = Vector3.Distance(forwardPos, startPos) / heavySpeed;
        elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(forwardPos, startPos, t);
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
