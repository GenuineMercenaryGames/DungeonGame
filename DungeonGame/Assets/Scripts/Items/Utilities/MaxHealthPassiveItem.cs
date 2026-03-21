using UnityEngine;

[CreateAssetMenu(menuName = "Roguelike/Passives/Max Health Item")]
public class MaxHealthPassiveItem : PassiveItemDefinitionBase
{
    private float healthIncrease;

    public override void OnAdded(PlayerItemSystem player)
    {
        // ACCESS HEALTH and ++healthIncrease
        //player.GetPlayerController().healthController.
    }
}