using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Hex
{
    public Vector3 position;
    public int q; // axial coordinate q
    public int r; // axial coordinate r

    public Hex(int q, int r)
    {
        this.q = q;
        this.r = r;
        this.position = AxialToPosition(q, r);
    }

    private Vector3 AxialToPosition(int q, int r)
    {
        float x = 3f / 2f * q;
        float y = Mathf.Sqrt(3) * (r + q / 2f);
        return new Vector3(x, 0, y);
    }
}

public class GridManager : MonoBehaviour
{
    public GameObject hexagon;
    public int size = 5;
    private float hexSize = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        GenerateHexagonalGrid(size);
    }

    void GenerateHexagonalGrid(int size)
    {
        for (int q = -size; q <= size; q++)
        {
            int r1 = Mathf.Max(-size, -q - size);
            int r2 = Mathf.Min(size, -q + size);
            for (int r = r1; r <= r2; r++)
            {
                Hex hex = new Hex(q, r);
                Vector3 position = hex.position;
                // rotate 90 degrees around the Y-axis
                GameObject hexObj = Instantiate(hexagon, position, Quaternion.Euler(0, 90, 0));
                hexObj.transform.localScale = new Vector3(hexSize, hexSize, hexSize);
            }
        }
    }
}