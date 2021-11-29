using UnityEngine;
using UnityEngine.UI;

public class MissionService : MonoBehaviour
{
    private GameObject missionTip;

    // Start is called before the first frame update
    void Start() {
        missionTip = GameObject.FindGameObjectWithTag("MissionTip");
    }

    // Update is called once per frame
    void Update() {
        ProcessMissionTip();
    }

    void ProcessMissionTip() {
        Text text = missionTip.GetComponent<Text>();
        if (!text) {
            return;
        }
        text.text = $"Kill 20/{GameService.kills} monsters";
    }

}

public enum MissionType {
    ELIMINATE_ALL,
    SURVIVE,
    OBJECT_CAPTURE,
}

public class EnemyItemTemplate {

    private MonsterType monsterType;
    private int maxHealth;
    private float speed;
    private string name;
    private RespawnType respawnType;
    private Weapons weapon;
    private int respawnInitialDelaySec;
    private int respawnDelaySec;
    private int totalCount;

}
