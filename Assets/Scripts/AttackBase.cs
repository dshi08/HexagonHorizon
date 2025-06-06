using System.Collections.Generic;
using UnityEngine;
public abstract class AttackBase : MonoBehaviour
{
    public abstract List<Vector2> HitTiles(Vector2 origin, Vector2 target);
    public abstract int damage { get; }
}
