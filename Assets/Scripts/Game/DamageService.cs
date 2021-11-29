using UnityEngine;

public static class DamageService {

    public static void ApplyDamage(PlayerInventory fromInventory, PlayerInventory toInventory) {
        if (fromInventory == null || toInventory == null) {
            return;
        }
        Weapons weapon = fromInventory.currentWeapon;
        WeaponProperties weaponProperties = WeaponService.GetWeaponProperties(weapon);
        int realDamage = GetRealDamage(toInventory, weaponProperties.Damage);
        //Debug.Log("realDamage=" + realDamage);
        if ((toInventory.health -= realDamage) <= 0) {
            toInventory.died = true;
        }
    }

    public static void ApplyDamage(PlayerInventory inventory, int damage) {
        if (!inventory) {
            return;
        }
        int realDamage = GetRealDamage(inventory, damage);
        if ((inventory.health -= realDamage) <= 0) {
            inventory.died = true;
        }
    }

    private static int GetRealDamage(PlayerInventory inventory, int baseDamage) {
        int armorPercent = inventory.armorPercent;
        if (armorPercent > 75) {
            armorPercent = 75;
        }
        if (armorPercent < 0) {
            armorPercent = 0;
        }
        if (!inventory.shield) {
            armorPercent = 0;
        }
        int blockedDamage = baseDamage * armorPercent / 100;
        //Debug.Log("baseDamage=" + baseDamage);
        //Debug.Log("blockedDamage=" + blockedDamage);
        return baseDamage - blockedDamage;
    }

}
