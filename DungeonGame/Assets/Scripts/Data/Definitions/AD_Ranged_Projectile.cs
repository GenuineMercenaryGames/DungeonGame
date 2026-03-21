using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackDefinition", menuName = "TheSweeper/Attack/Ranged/Projectile")]
public class AD_Ranged_Projectile : AttackDefinition
{
    public GameObject ProjectilePrefab;

    public override void AttackBegin()
    {
        var projectile = Instantiate(ProjectilePrefab);
    }

    public override void AttackEnd()
    {

    }

    public override void AttackTick()
    {

    }
}
