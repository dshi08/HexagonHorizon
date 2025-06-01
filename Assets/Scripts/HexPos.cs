// HexPos.cs
using UnityEngine;

public class HexPos : MonoBehaviour
{
    public int q; // axial coordinate q
    public int r; // axial coordinate r

    // Read-only property that converts axial coords to world position
    // Uses the same calculation as the reference code
    public Vector3 position
    {
        get => AxialToPosition(q, r);
    }

    /// <summary>
    /// Returns the axial coordinates as a Vector2Int.
    /// </summary>
    public Vector2Int GetPos()
    {
        return new Vector2Int(q, r);
    }

    /// <summary>
    /// Sets the axial coordinates of this hex.
    /// </summary>
    /// <param name="q">The q-coordinate.</param>
    /// <param name="r">The r-coordinate.</param>
    public void SetPos(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    /// <summary>
    /// Converts axial coordinates to a world position.
    /// Uses the exact same formula as the reference code for consistent spacing.
    /// </summary>
    /// <param name="q">The q-coordinate.</param>
    /// <param name="r">The r-coordinate.</param>
    /// <returns>The calculated world position.</returns>
    private Vector3 AxialToPosition(int q, int r)
    {
        // This matches the reference code exactly for consistent spacing
        float x = 3f / 2f * q;
        float y = Mathf.Sqrt(3) * (r + q / 2f);
        return new Vector3(x, 0, y);
    }
}
