// GridManager.cs
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

    private HashSet<string> currentlyRevealedHexKeys = new HashSet<string>();

    void Start()
    {
        GenerateHexagonalGrid(size);
        Reveal(0, 0, 1);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                GameObject clickedHex = hit.collider.gameObject;
                HexPos hex = clickedHex.GetComponent<HexPos>();
                
                if (hex != null)
                {
                    player.GetComponent<PlayerMovement>()?.MoveToPos(hex.q, hex.r);
                    Reveal(hex.q, hex.r, 1);
                }
                else
                {
                    Debug.LogWarning("Clicked object '" + clickedHex.name + "' does not have a HexPos component or is not a hex.");
                }
            }
        }
    }

    void Reveal(int centerQ, int centerR, int radius)
    {
        HashSet<string> hexesToRevealThisTurn = new HashSet<string>();

        // Calculate which hexes should be revealed
        for (int q = -radius; q <= radius; q++)
        {
            int r1 = Mathf.Max(-radius, -q - radius);
            int r2 = Mathf.Min(radius, -q + radius);
            for (int r = r1; r <= r2; r++)
            {
                string key = $"{centerQ + q},{centerR + r}";
                if (hexes.ContainsKey(key))
                {
                    hexesToRevealThisTurn.Add(key);
                }
            }
        }

        // Hide hexes that should no longer be revealed
        HashSet<string> hexesToHide = new HashSet<string>(currentlyRevealedHexKeys);
        hexesToHide.ExceptWith(hexesToRevealThisTurn);
        
        foreach (string key in hexesToHide)
        {
            if (hexes.TryGetValue(key, out GameObject hexObj))
            {
                Animator anim = hexObj.GetComponent<Animator>();
                anim.SetBool("Flipped", false);
                // hexObj.transform.rotation = Quaternion.Euler(0, 90, 0);
            }
            currentlyRevealedHexKeys.Remove(key);
        }

        // Reveal only the new hexes
        HashSet<string> hexesToShow = new HashSet<string>(hexesToRevealThisTurn);
        hexesToShow.ExceptWith(currentlyRevealedHexKeys);
        
        foreach (string key in hexesToShow)
        {
            if (hexes.TryGetValue(key, out GameObject hexObj))
            {
                Animator anim = hexObj.GetComponent<Animator>();
                anim.SetBool("Flipped", true);
                // hexObj.transform.rotation = Quaternion.Euler(180, 90, 0);
                currentlyRevealedHexKeys.Add(key);
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
                // Create hex with initial rotation (matches reference code)
                GameObject hexObj = Instantiate(hexagon, transform.position, Quaternion.Euler(0, 90, 0));
                hexes[$"{q},{r}"] = hexObj;
                
                // Apply scaling using localScale (matches reference code)
                hexObj.transform.localScale = new Vector3(hexSize, hexSize, hexSize);
                
                // Add HexPos component and set coordinates
                HexPos pos = hexObj.GetComponent<HexPos>();
                if (pos == null)
                {
                    pos = hexObj.AddComponent<HexPos>();
                }
                pos.q = q;
                pos.r = r;
                
                // Position hex using HexPos.position property (matches reference code)
                hexObj.transform.position = pos.position;
                hexObj.name = $"Hex_{q},{r}";

                // Initialize animation state
                Animator anim = hexObj.GetComponent<Animator>();
                anim.SetBool("Flipped", false);
            }
        }
    }
}
