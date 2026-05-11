using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectMenuController : MonoBehaviour
{
    [System.Serializable]
    public struct WeaponSelectionData
    {
        public string weaponName;
        public Transform visualMesh;
        public GameObject weaponPrefab;
        public bool isUnlocked;
    }

    [SerializeField] private Transform lockedPopUp;
    [SerializeField] private LocalizedText weaponNameLocalizer;
    [SerializeField] private LocalizedText weaponDescLocalizer;
    [SerializeField] private int defaultWeaponIndex;

    [SerializeField] public WeaponSelectionData[] weaponData;

    void Start()
    {
        SelectWeapon(defaultWeaponIndex);
    }

    private void SetWeaponActive(int index, bool active)
    {
        weaponData[index].visualMesh.gameObject.SetActive(active);
        if (active)
        {
            weaponNameLocalizer.LOC = $"loc_weapon_name_{weaponData[index].weaponName}";
            weaponDescLocalizer.LOC = $"loc_weapon_desc_{weaponData[index].weaponName}";
            lockedPopUp.gameObject.SetActive(!weaponData[index].isUnlocked);
        }
    }

    private void HideAllWeapons()
    {
        lockedPopUp.gameObject.SetActive(false);
        for (int i = 0; i < weaponData.Length; i++)
            SetWeaponActive(i, false);
    }

    public void SelectWeapon(int index)
    {
        HideAllWeapons();
        SetWeaponActive(index, true);
    }

    /*
    public void SelectPrimaryWeapon(int index)
    {
        SelectWeapon(index);
        GameConfigManager.Instance.selectedWeaponPrimary = weaponData[index].weaponPrefab;
    }

    public void SelectSecondaryWeapon(int index)
    {
        SelectWeapon(index);
        GameConfigManager.Instance.selectedWeaponSecondary = weaponData[index].weaponPrefab;
    }
    */

}
