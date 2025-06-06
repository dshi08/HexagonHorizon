// GridManager.cs
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject hexagon;
    public int size = 5;
    private float hexSize = 0.85f;
    public GameObject player;
    private PlayerMovement playerMovement;
    private AttackManager attackManager;
    public Dictionary<string, GameObject> hexes = new Dictionary<string, GameObject>();
    private Dictionary<string, bool> occupiedHexes = new Dictionary<string, bool>();
    private HashSet<string> currentlyRevealedHexKeys = new HashSet<string>();
    public GameObject enemyManagerObject;
    private EnemyManager enemyManager;
    public int? currentAttackIndex = null;
    public int revealRadius = 1;

    void Start()
    {
        GenerateHexagonalGrid(size);
        Reveal(0, 0, 1);
        enemyManager = enemyManagerObject.GetComponent<EnemyManager>();
        attackManager = player.GetComponent<AttackManager>();
        playerMovement = player.GetComponent<PlayerMovement>();
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
                    HandleMove(hex);
                }
                else
                {
                    Debug.LogWarning("Clicked object '" + clickedHex.name + "' does not have a HexPos component or is not a hex.");
                }
            }
        }
    }

    void HandleMove(HexPos hex)
    {
        if (currentAttackIndex == null) return;

        int dQ = playerMovement.currentHex.q - hex.q;
        int dR = playerMovement.currentHex.r - hex.r;
        bool isAdjacent = Math.Abs(dQ) <= 1 && Math.Abs(dR) <= 1 && Math.Abs(dQ + dR) <= 1 && !(dQ == 0 && dR == 0);
        if (currentAttackIndex == -1)
        {
            // We want to move
            if (!IsHexAvailable(hex.q, hex.r) || !isAdjacent) return;

            playerMovement.MoveToPos(hex.q, hex.r);
            Reveal(hex.q, hex.r, 1);
        }
        else
        {
            if (currentAttackIndex == 0 && !isAdjacent) return;
            attackManager.Attack(playerMovement.currentHex.GetPos(), hex.GetPos(), currentAttackIndex.Value);
            if (currentAttackIndex == 0) // Ram
            {
                playerMovement.MoveToPos(hex.q, hex.r);
                Reveal(hex.q, hex.r, 1);
            }
        }

        currentAttackIndex = null;
        Invoke("MoveEnemies", 0.5f); // Delay enemy movement to allow player action to complete
    }

    void MoveEnemies()
    {
        enemyManager.Move();
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
                occupiedHexes[$"{q},{r}"] = false; // Initialize as unoccupied

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

    public void LeaveHex(int q, int r)
    {
        occupiedHexes[$"{q},{r}"] = false;
    }

    public void EnterHex(int q, int r)
    {
        occupiedHexes[$"{q},{r}"] = true;
    }

    public bool IsHexAvailable(int q, int r)
    {
        return !occupiedHexes.ContainsKey($"{q},{r}") || !occupiedHexes[$"{q},{r}"];
    }

    public void IncreaseRevealRadius()
    {
        revealRadius++;
        HexPos playerPos = player.GetComponent<PlayerMovement>().currentHex;
        Reveal(playerPos.q, playerPos.r, revealRadius);
    }
}
