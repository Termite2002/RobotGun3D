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

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }

        return false;
    }

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
}
