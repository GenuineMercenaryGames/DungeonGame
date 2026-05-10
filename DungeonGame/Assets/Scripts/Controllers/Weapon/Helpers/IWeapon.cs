using UnityEngine;

public interface IWeapon
{
    public void ShootBegin();
    public void ShootEnd();
    public void ShootTick(float dt);
    public void IdleTick(float dt);

    public bool CanShoot();

    public int GetAmmo();
    public void SetAmmo(int ammo);

    // public int Ammo();
    // public AmmoType AmmoType();
}
