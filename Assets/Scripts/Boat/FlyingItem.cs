using UnityEngine;
using System.Collections;

public class FlyingItem : MonoBehaviour
{
    public void StartFlight(Transform start, Transform target, float duration)
    {
        StartCoroutine(FlyRoutine(start.position, target.position, duration));
    }

    IEnumerator FlyRoutine(Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsed = 0;
        Vector3 controlPoint = (startPos + endPos) / 2 + Vector3.up * 3f; // De "hoogte" van de boog

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Bezier curve voor een mooie boog
            Vector3 m1 = Vector3.Lerp(startPos, controlPoint, t);
            Vector3 m2 = Vector3.Lerp(controlPoint, endPos, t);
            transform.position = Vector3.Lerp(m1, m2, t);

            // Laat de kubus ook een beetje draaien voor effect
            transform.Rotate(Vector3.one * 500f * Time.deltaTime);

            yield return null;
        }

        Destroy(gameObject); // Verwijder kubus als hij bij de boot is
    }
}