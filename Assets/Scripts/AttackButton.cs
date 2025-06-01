using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackButton : MonoBehaviour
{
    public GameObject gridManagerObject;
    public int attackIndex;
    private GridManager gridManager;

    void Start()
    {
        gridManager = gridManagerObject.GetComponent<GridManager>();
        GetComponent<Button>().onClick.AddListener(PrimeAttack);
    }

    void PrimeAttack()
    {
        gridManager.currentAttackIndex = attackIndex;
    }
}
