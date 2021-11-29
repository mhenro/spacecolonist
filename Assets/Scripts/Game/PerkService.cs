using UnityEngine;

public class PerkService : MonoBehaviour {

    public PerkItem perkItem1;
    public PerkItem perkItem2;
    public PerkItem perkItem3;

    private GameObject player;
    private PlayerInventory inventory;
    private GameObject perkWindow;
    private int currentLevel;
    private TimeManager timeManager;

    // Start is called before the first frame update
    void Start() {
        perkWindow = GameObject.FindGameObjectWithTag("PerkWindow");
        perkWindow.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<PlayerInventory>();
        currentLevel = 1;
        timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update() {
        ProcessLevelUp();
    }

    void ProcessLevelUp() {
        if (currentLevel >= GameService.level) {
            return;
        }
        currentLevel = GameService.level;
        ShowChoosePerkWindow();
    }

    void ShowChoosePerkWindow() {
        PerkType perk1 = CreateRandomPerk();
        PerkType perk2 = CreateRandomPerk();
        while (perk1 == perk2) {
            perk2 = CreateRandomPerk();
        }
        PerkType perk3 = CreateRandomPerk();
        while (perk3 == perk2 || perk3 == perk1) {
            perk3 = CreateRandomPerk();
        }
        perkItem1.perkType = perk1;
        perkItem1.perkDescription = GetPerkDescription(perk1);
        perkItem2.perkType = perk2;
        perkItem2.perkDescription = GetPerkDescription(perk2);
        perkItem3.perkType = perk3;
        perkItem3.perkDescription = GetPerkDescription(perk3);
        perkWindow.SetActive(true);
        timeManager.PauseGame();
    }

    public void ApplyPerkToPlayer(PerkType perkType) {
        switch (perkType) {
            case PerkType.LASER_SIGHT: {
                inventory.laserSight = true;
                break;
            } 
            case PerkType.INCREASE_50_HP: {
                inventory.maxHealth += 50;
                break;
            }
            case PerkType.INCREASE_100_HP: {
                inventory.maxHealth += 100;
                break;
            }
            case PerkType.INCREASE_10_SEC_SHIELD: {
                inventory.armorPersistSec += 10;
                break;
            }
            case PerkType.INCREASE_10_PERCENT_SHIELD_ARMOR: {
                inventory.armorPercent += 10;
                break;
            }
        }
    }

    public PerkType CreateRandomPerk() {
        return (PerkType)Random.Range(0, System.Enum.GetValues(typeof(PerkType)).Length);
    }

    public string GetPerkDescription(PerkType perkType) {
        switch (perkType) {
            case PerkType.LASER_SIGHT: {
                return "Laser sight";
            }
            case PerkType.INCREASE_50_HP: {
                return "Increase 50 hp";
            }
            case PerkType.INCREASE_100_HP: {
                return "Increase 100 hp";
            }
            case PerkType.INCREASE_10_SEC_SHIELD: {
                return "Increase shield time to 10 seconds";
            }
            case PerkType.INCREASE_10_PERCENT_SHIELD_ARMOR: {
                return "Increase shield armor to 10%";
            }
        }
        return "";
    }

}

public enum PerkType {
    LASER_SIGHT,
    INCREASE_50_HP,
    INCREASE_100_HP,
    INCREASE_10_SEC_SHIELD,
    INCREASE_10_PERCENT_SHIELD_ARMOR,
}