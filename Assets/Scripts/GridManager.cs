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

    // Start is called before the first frame update
    void Start()
    {
        GenerateHexagonalGrid(size);
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
                hexObj.transform.localScale = new Vector3(hexSize, hexSize, hexSize);
                HexPos pos = hexObj.AddComponent<HexPos>();
                pos.q = q;
                pos.r = r;
                hexObj.transform.position = pos.position;
            }
        }
    }
}