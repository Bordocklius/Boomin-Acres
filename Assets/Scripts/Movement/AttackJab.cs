using UnityEngine;

public class AttackJab : Attack
{
    [SerializeField]
    private GameObject jabDest;
    [SerializeField]
    private float jabSpeed;
    public override void OnAttack()
    {
        attachedMovementHandler.enabled = false;
        StartCoroutine(JabCoroutine());
    }

    private System.Collections.IEnumerator JabCoroutine()
    {
        Vector3 startPos = handObject.transform.position;
        Vector3 destPos = jabDest.transform.position;
        float elapsedTime = 0f;
        //multiply jabspeed by a global multiplier if you boosting
        float totalTime = Vector3.Distance(startPos, destPos) / jabSpeed;

        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(startPos, destPos, t);
            yield return null;
        }
        handObject.transform.position = destPos;

        elapsedTime = 0f;
        while (elapsedTime < totalTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / totalTime);
            handObject.transform.position = Vector3.Lerp(destPos, startPos, t);
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
