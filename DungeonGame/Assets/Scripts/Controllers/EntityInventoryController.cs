using UnityEngine;

public class EntityInventoryController : MonoBehaviour
{
    // TODO : Finish implementing logic for ammo tracking. The in-game inventory is pretty bare-bones because the player just has to keep track of their currently equiped weapon and money amount.
    [SerializeField] public int Money;
    [SerializeField] public int[] Ammo;
}
