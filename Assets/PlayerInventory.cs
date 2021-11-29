using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public bool player = false;
    public int maxHealth = 100;
    public int health = 100;
    public int expForDead = 0;
    public float speed = 4f;
    public bool died = false;
    public Weapons currentWeapon;
    public int bulletCount;
    public bool laserSight;
    public bool shield = false;
    public int armorPercent = 0;
    public int armorPersistSec = 0; //-1 - infinite
    public PerkItem[] perks;
}
