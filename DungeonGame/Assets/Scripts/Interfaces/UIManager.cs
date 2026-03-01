using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Bars")]
    public Image healthFill;
    public Image energyFill;

    [Header("Weapon Slots")]
    public Image primaryIcon;
    public Image secondaryIcon;

    public void UpdateHealth(float current, float max)
    {
        healthFill.fillAmount = current / max;
    }

    public void UpdateEnergy(float current, float max)
    {
        energyFill.fillAmount = current / max;
    }

    public void SetPrimaryWeapon(Sprite icon)
    {
        primaryIcon.sprite = icon;
        primaryIcon.enabled = true;
    }

    public void SetSecondaryWeapon(Sprite icon)
    {
        secondaryIcon.sprite = icon;
        secondaryIcon.enabled = true;
    }
}