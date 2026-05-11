using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectMenuController : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponSelectionData
    {
        public Transform mesh;
        public Image button;
        public string locName;
        public string locDesc;
        public GameObject weapon;
    }

    [SerializeField] private TMP_Text weaponName;
    [SerializeField] private TMP_Text weaponDescription;

    [SerializeField] public WeaponSelectionData[] weaponsPrimary;
    [SerializeField] public WeaponSelectionData[] weaponsSecondary;

    void Start()
    {
        HideAllWeapons();
    }

    private void SetWeaponMeshActiveFromList(WeaponSelectionData[] weapons, int index, bool active)
    {
        weapons[index].mesh.gameObject.SetActive(active);
        weapons[index].button.color = active ? Color.yellow : Color.white;
    }

    private void HideAllWeaponsFromList(WeaponSelectionData[] weapons)
    {
        for (int i = 0; i < weapons.Length; i++)
            SetWeaponMeshActiveFromList(weapons, i, false);
    }

    private void SelectWeaponFromList(WeaponSelectionData[] weapons, int index)
    {
        HideAllWeaponsFromList(weapons);
        SetWeaponMeshActiveFromList(weapons, index, true);
    }

    private void HideAllPrimaryWeapons()
    {
        HideAllWeaponsFromList(weaponsPrimary);
    }

    private void HideAllSecondaryWeapons()
    {
        HideAllWeaponsFromList(weaponsSecondary);
    }

    private void HideAllWeapons()
    {
        HideAllPrimaryWeapons();
        HideAllSecondaryWeapons();
    }

    public void SelectPrimaryWeapon(int index)
    {
        SelectWeaponFromList(weaponsPrimary, index);
    }

    public void SelectSecondaryWeapon(int index)
    {
        SelectWeaponFromList(weaponsSecondary, index);
    }

}
