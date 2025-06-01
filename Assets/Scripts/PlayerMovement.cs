using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private HexPos currentHex;

    // Start is called before the first frame update
    void Start()
    {
        currentHex = GetComponent<HexPos>();
    }

    public void MoveToPos(int q, int r)
    {
        currentHex.SetPos(q, r);
        Vector3 newPosition = currentHex.position;

        // Tween to the new position in an arc, as if moving a chess piece
        StartCoroutine(MoveToPosition(newPosition));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        float duration = 1f; // Duration of the movement

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // Use a quadratic curve for the arc
            float height = Mathf.Sin(t * Mathf.PI) * 0.5f; // Adjust height as needed
            transform.position = Vector3.Lerp(startPosition, targetPosition, t) + new Vector3(0, height, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure final position is set

    }

}
