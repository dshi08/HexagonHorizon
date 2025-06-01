using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    public int health = 10;
    public GridManager gridManager; 
    public HexPos hexPos;
    public float moveDuration = 0.5f; // Match player's movement speed
    public float arcHeight = 0.3f;    // Slightly lower arc than player for visual distinction

    void Start()
    {
        hexPos = GetComponent<HexPos>();
    }

    // Public method to move enemy to axial coordinates (q, r)
    public void MoveToHex(int q, int r)
    {
        gridManager.LeaveHex(hexPos.q, hexPos.r);
        gridManager.EnterHex(q, r);
        Vector3 startPos = hexPos.position;
        hexPos.SetPos(q, r);

        if (!gameObject.activeSelf) return;
        StartCoroutine(MoveToPosition(startPos, hexPos.position));
    }

    // Smooth arc movement (identical to player's style)
    private IEnumerator MoveToPosition(Vector3 startPosition, Vector3 targetPosition)
    {
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t) + Vector3.up * height;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Snap to final position
    }

    // --- AI Logic ---
    public void ChasePlayer(HexPos playerHex)
    {
        Vector2Int[] neighbors = GetNeighbors();
        Vector2Int bestMove = hexPos.GetPos();
        int minDistance = int.MaxValue;

        foreach (Vector2Int neighbor in neighbors)
        {
            int dist = HexDistance(neighbor, playerHex.GetPos());
            if (dist < minDistance && gridManager.IsHexAvailable(neighbor.x, neighbor.y))
            {
                minDistance = dist;
                bestMove = neighbor;
            }
        }

        bool canSee = minDistance <= 1;
        // set visibility
        gameObject.SetActive(canSee);

        MoveToHex(bestMove.x, bestMove.y);
    }

    // Get all 6 neighboring hex positions
    private Vector2Int[] GetNeighbors()
    {
        return new Vector2Int[]
        {
            new Vector2Int(hexPos.q + 1, hexPos.r),     // East
            new Vector2Int(hexPos.q + 1, hexPos.r - 1), // Northeast
            new Vector2Int(hexPos.q, hexPos.r - 1),     // Northwest
            new Vector2Int(hexPos.q - 1, hexPos.r),     // West
            new Vector2Int(hexPos.q - 1, hexPos.r + 1), // Southwest
            new Vector2Int(hexPos.q, hexPos.r + 1)      // Southeast
        };
    }

    // Hex distance calculation (axial coordinates)
    private int HexDistance(Vector2Int a, Vector2Int b)
    {
        int s1 = -a.x - a.y;
        int s2 = -b.x - b.y;
        return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(s1 - s2)) / 2;
    }

    // Helper to get current axial position
    public Vector2Int GetPos()
    {
        return new Vector2Int(hexPos.q, hexPos.r);
    }
}