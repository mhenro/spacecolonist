using System;
using UnityEngine;
using System.Reflection;

public static class WeaponService {

    public const string PISTOL_IMAGE_PATH = "Icons/PistolIcon";
    //public const string SHOTGUN_IMAGE_PATH = "Icons/MachinegunIcon";
    public const string SHOTGUN_IMAGE_PATH = "Icons/ShotgunIcon";

    public static Sprite PISTOL_SPRITE = Resources.Load<Sprite>(PISTOL_IMAGE_PATH);
    public static Sprite SHOTGUN_SPRITE = Resources.Load<Sprite>(SHOTGUN_IMAGE_PATH);

    public const string CLAWS_FIRE_FX = "ClawsFire";
    public const string PISTOL_FIRE_FX = "PistolFire";
    public const string SHOTGUN_FIRE_FX = "ShotgunFire";

    public static void TakeWeapon(GameObject person, Weapons weapon) {
        PlayerInventory inventory = person.GetComponent<PlayerInventory>();
        if (inventory == null) {
            return;
        }
        inventory.currentWeapon = weapon;
        WeaponProperties weaponProperties = GetWeaponProperties(weapon);
        inventory.bulletCount = weaponProperties.BulletCount;
    }

    public static WeaponProperties GetWeaponProperties(Weapons weapon) {
        return (WeaponProperties)Attribute.GetCustomAttribute(ForValue(weapon), typeof(WeaponProperties));
    }

    private static MemberInfo ForValue(Weapons weapon) {
        return typeof(Weapons).GetField(Enum.GetName(typeof(Weapons), weapon));
    }

    public static Sprite GetWeaponSprite(Weapons weapon) {
        Sprite sprite; 
        switch (weapon) {
            case Weapons.PISTOL: sprite = PISTOL_SPRITE; break;
            case Weapons.SHOTGUN: sprite = SHOTGUN_SPRITE; break;
            default: sprite = null; break;
        }
        return sprite;
    }

}

public class WeaponProperties: Attribute {

    /**
     * damage - in xp points
     * bulletCount - -1 - infinity
     * distance - 0 - melee
     * description - string
     * */
    internal WeaponProperties(int damage, int bulletCount, float distance, string description, string weaponImage, string fireFx) {
        this.Damage = damage;
        this.BulletCount = bulletCount;
        this.Distance = distance;
        this.Description = description;
        this.WeaponImage = weaponImage;
        this.FireFx = fireFx;
    }

    public int Damage { get; private set; }
    public int BulletCount{ get; private set; }
    public float Distance { get; private set; }
    public string Description { get; private set; }
    public string WeaponImage { get; private set; }
    public string FireFx { get; private set; }

}

public enum Weapons {
    [WeaponProperties(3, -1, 0f, "CLAWS", "", WeaponService.CLAWS_FIRE_FX)] CLAWS1,
    [WeaponProperties(7, -1, 0f, "CLAWS2", "", WeaponService.CLAWS_FIRE_FX)] CLAWS2,
    [WeaponProperties(3, -1, 10f, "PISTOL", WeaponService.PISTOL_IMAGE_PATH, WeaponService.PISTOL_FIRE_FX)] PISTOL,
    [WeaponProperties(20, 20, 3f, "SHOTGUN", WeaponService.SHOTGUN_IMAGE_PATH, WeaponService.SHOTGUN_FIRE_FX)] SHOTGUN,
}
