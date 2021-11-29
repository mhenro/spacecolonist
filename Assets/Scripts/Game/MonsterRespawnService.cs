using UnityEngine;

public class MonsterRespawnService : MonoBehaviour
{
    public Mission mission;
    public GameObject arachnidPrefab;
    public GameObject slugPrefab;

    private ObjectRespawner respawner;
    private GameObject planet;
    private float nextTimeGeneration;

    // Start is called before the first frame update
    void Start() {
        respawner = GetComponent<ObjectRespawner>();
        planet = GameObject.FindGameObjectWithTag("Planet");
        nextTimeGeneration = 0f;
    }

    // Update is called once per frame
    void Update() {
        CreateMonsters();
        CreateBosses();
    }

    void CreateMonsters() {
        if (Time.time <= nextTimeGeneration) {
            return;
        }
        EventType eventType = GetRandomEventType();
        switch (eventType) {
            case EventType.NEW_SMALL_ARACHNID_EVENT: {
                CreateSmallArachnid();
                break;
            }
            case EventType.NEW_SMALL_SLUG_EVENT: {
                CreateSmallSlug();
                break;
            }
        }
        nextTimeGeneration = Time.time + Random.Range(0f, 10f);
    }

    void CreateSmallArachnid() {
        Gravitable gravitable = arachnidPrefab.GetComponent<Gravitable>();
        if (gravitable) {
            gravitable.planet = planet;
        }
        GameObject arachnid = respawner.CreateObjectOnPlanet(arachnidPrefab, RespawnType.OVER_THE_HORIZON);
        arachnid.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        PlayerInventory inventory = arachnid.GetComponent<PlayerInventory>();
        inventory.player = false;
        inventory.maxHealth = 6;
        inventory.health = 6;
        inventory.expForDead = 50;
        inventory.speed = 0.15f;
        inventory.currentWeapon = Weapons.CLAWS1;
        inventory.armorPercent = 0;
    }

    void CreateSmallSlug() {
        Gravitable gravitable = slugPrefab.GetComponent<Gravitable>();
        if (gravitable) {
            gravitable.planet = planet;
        }
        GameObject slug = respawner.CreateObjectOnPlanet(slugPrefab, RespawnType.OVER_THE_HORIZON);
        slug.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        PlayerInventory inventory = slug.GetComponent<PlayerInventory>();
        inventory.player = false;
        inventory.maxHealth = 10;
        inventory.health = 10;
        inventory.expForDead = 100;
        inventory.speed = 0.11f;
        inventory.currentWeapon = Weapons.CLAWS2;
        inventory.shield = true;
        inventory.armorPercent = 10;
        inventory.armorPersistSec = -1; //infinite
    }

    void CreateBosses() {

    }

    EventType GetRandomEventType() {
        return (EventType)Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
    }

    enum EventType {
        NEW_SMALL_ARACHNID_EVENT,
        NEW_SMALL_SLUG_EVENT,
    }

}
