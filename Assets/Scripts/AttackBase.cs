using UnityEngine;
public abstract class AttackBase : MonoBehaviour
{
    public abstract void Execute();
    public abstract KeyCode hotkey { get; }
    public abstract string attackName { get; }
    public abstract int damage { get; }
}
