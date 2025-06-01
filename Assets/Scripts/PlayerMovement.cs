using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject gridManagerObject;
    private GridManager gridManager;
    public HexPos currentHex;

    // Start is called before the first frame update
    void Start()
    {
        currentHex = GetComponent<HexPos>();
        gridManager = gridManagerObject.GetComponent<GridManager>();
        gridManager.EnterHex(currentHex.q, currentHex.r);
    }

    public void MoveToPos(int q, int r)
    {
        gridManager.LeaveHex(currentHex.q, currentHex.r);
        gridManager.EnterHex(q, r);
        currentHex.SetPos(q, r);
        Vector3 newPosition = currentHex.position;

        // Tween to the new position in an arc, as if moving a chess piece
        StartCoroutine(MoveToPosition(newPosition));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        // face towards where we're going
        Vector3 direction = targetPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        float duration = 0.5f; // Duration of the movement

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            // Use a quadratic curve for the arc
            float height = Mathf.Sin(t * Mathf.PI) * -0.25f; // Adjust height as needed
            transform.position = Vector3.Lerp(startPosition, targetPosition, t) + new Vector3(0, height, 0);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure final position is set
    }
}
