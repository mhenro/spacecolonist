using UnityEngine;

public class GameService : MonoBehaviour
{
    public static int level = 0;
    public static int currentExp = 0;
    public static int levelUpExp = 0;
    public static int kills = 0;
    public static int shots = 0;
    //TODO: calc the accuracy like kills / shots
    public static float startTime = 0;
    public static float endTime = 0;

    private GameObject player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        PrepareNewGame();
    }

    public void PrepareNewGame() {
        PreparePlayer();
        PrepareLevel();
        PrepareStatistic();
        FindObjectOfType<AudioManager>().Play("FightTheme");

    }

    public void PreparePlayer() {
        if (!player) {
            return;
        }
        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        inventory.player = true;
        inventory.maxHealth = 100;
        inventory.health = 100;
        inventory.expForDead = 0;
        inventory.speed = 0.5f;
        inventory.died = false;
        inventory.currentWeapon = Weapons.PISTOL;
        WeaponProperties weaponProperties = WeaponService.GetWeaponProperties(Weapons.PISTOL);
        inventory.bulletCount = weaponProperties.BulletCount;
        inventory.laserSight = false;
        inventory.shield = false;
        inventory.armorPercent = 10;
        inventory.armorPersistSec = 10;
    }

    void PrepareLevel() {
        level = 1;
        currentExp = 0;
        levelUpExp = 50;
    }

    void PrepareStatistic() {
        kills = 0;
        shots = 0;
        startTime = Time.time;
    }

    void Update() {
        ProcessLevel();    
    }

    void ProcessLevel() {
        if (levelUpExp <= currentExp) {
            currentExp = 0;
            ++level;
            levelUpExp = level * 500 + (int)GetAccuracy() * 100;
        }
    }

    float GetAccuracy() {
        return kills / shots;
    }

}
