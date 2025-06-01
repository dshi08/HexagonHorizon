using Unity.VisualScripting;
using UnityEngine;

public class HexPos : MonoBehaviour
{
    public int q; // axial coordinate q
    public int r; // axial coordinate r

    public Vector3 position
    {
        get => AxialToPosition(q, r);
    }

    public void SetPos(int q, int r)
    {
        this.q = q;
        this.r = r;
    }

    private Vector3 AxialToPosition(int q, int r)
    {
        float x = 3f / 2f * q;
        float y = Mathf.Sqrt(3) * (r + q / 2f);
        return new Vector3(x, 0, y);
    }
}