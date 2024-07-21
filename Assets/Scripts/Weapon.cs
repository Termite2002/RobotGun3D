using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}

[System.Serializable]
public class Weapon 
{
    public WeaponType weaponType;

    public int bulletsInMagazine;
    public int magazineCapacity;

    public int totalReserveAmmo;

    [Range(1, 3)]
    public float reloadSpeed = 1;
    [Range(1, 3)]
    public float equipmentSpeed = 1;

    [Space]
    public float fireRate = 1; // bullet/second
    private float lastShootTime;
    public bool CanShoot()
    {
        if (HaveEnoughBullets() && ReadyToFire())
        {
            bulletsInMagazine--;
            return true;
        }

        return false;
    }
    private bool ReadyToFire()
    {
        if (Time.time > lastShootTime + 1 / fireRate)
        {
            lastShootTime = Time.time;
            return true;
        }

        return false;
    }



    #region Reload methods
    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
            return false;

        return totalReserveAmmo > 0; 
    }

    public void RefillBullets()
    {
        int bulletsToReaload = magazineCapacity;

        if (bulletsToReaload > totalReserveAmmo)
        {
            bulletsToReaload = totalReserveAmmo;
        }

        totalReserveAmmo -= bulletsToReaload;
        bulletsInMagazine = bulletsToReaload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }
    private bool HaveEnoughBullets() => bulletsInMagazine > 0;
    #endregion
}
