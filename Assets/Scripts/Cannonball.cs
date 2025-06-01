using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public void Shoot(Vector3 startPos, Vector3 endPos, float duration = 0.5f)
    {
        StartCoroutine(MoveCannonball(startPos, endPos, duration));
    }

    private IEnumerator MoveCannonball(Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float height = Mathf.Sin(t * Mathf.PI) * 0.3f; // Arc height
            transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * height;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos; // Snap to final position
        Destroy(gameObject); // Destroy the cannonball after it reaches its target
    }
}
