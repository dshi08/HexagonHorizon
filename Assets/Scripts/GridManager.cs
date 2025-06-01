using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject hexagon;
    public int size = 5;
    private float hexSize = 0.8f;
    public GameObject player;
    private Dictionary<string, GameObject> hexes = new Dictionary<string, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateHexagonalGrid(size);
        Reveal(0, 0, 1);
    }

    void Update()
    {
        // get which hexagon is clicked
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject clickedHex = hit.collider.gameObject;
                HexPos hex = clickedHex.GetComponent<HexPos>();
                player.GetComponent<PlayerMovement>().MoveToPos(hex.q, hex.r);
                Reveal(hex.q, hex.r, 1);
            }
        }
    }

    void Reveal(int centerQ, int centerR, int radius)
    {
        // Rotate the hexagons 180 degrees around the x axis
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                if (hexes.TryGetValue($"{centerQ + q},{centerR + r}", out GameObject hexObj))
                {
                    hexObj.transform.rotation = Quaternion.Euler(180, 90, 0);
                }
            }
        }
    }

    void GenerateHexagonalGrid(int size)
    {
        for (int q = -size; q <= size; q++)
        {
            int r1 = Mathf.Max(-size, -q - size);
            int r2 = Mathf.Min(size, -q + size);
            for (int r = r1; r <= r2; r++)
            {
                // rotate 90 degrees around the Y-axis
                GameObject hexObj = Instantiate(hexagon, transform.position, Quaternion.Euler(0, 90, 0));
                hexes[$"{q},{r}"] = hexObj;
                hexObj.transform.localScale = new Vector3(hexSize, hexSize, hexSize);
                HexPos pos = hexObj.AddComponent<HexPos>();
                pos.q = q;
                pos.r = r;
                hexObj.transform.position = pos.position;
            }
        }
    }
}