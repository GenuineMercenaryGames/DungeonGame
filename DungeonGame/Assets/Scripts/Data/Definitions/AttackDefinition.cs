using UnityEngine;

public abstract class AttackDefinition : ScriptableObject
{
    public abstract void AttackBegin();
    public abstract void AttackEnd();
    public abstract void AttackTick();
}
