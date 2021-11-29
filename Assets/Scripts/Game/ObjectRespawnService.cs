using UnityEngine;

public class ObjectRespawnService : MonoBehaviour {

    public GameObject shotgunPrefab;
    public GameObject medikitPrefab;
    public GameObject shieldPrefab;
    public GameObject grenadePrefab;
    public GameObject clockPrefab;
    public GameObject itemAuraPrefab;

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
    void Update()
    {
        CreateRandomEvent();
    }

    private void CreateRandomEvent() {
        if (Time.time <= nextTimeGeneration) {
            return;
        }
        EventType eventType = GetRandomEventType();
        switch (eventType) {
            case EventType.NEW_SHOTGUN_EVENT: {
                CreateShotgun();
                break;
            }
            case EventType.NEW_MEDIKIT_EVENT: {
                CreateMedikit();
                break;
            }
            case EventType.NEW_SHIELD_EVENT: {
                CreateShield();
                break;
            }
            case EventType.NEW_GRENADE_EVENT: {
                CreateGrenade();
                break;
            }
            case EventType.NEW_CLOCK_EVENT: {
                CreateClock();
                break;
            }
        }
        nextTimeGeneration = Time.time + Random.Range(0f, 50f);
    }

    void CreateShotgun() {
        Gravitable gravitable = shotgunPrefab.GetComponent<Gravitable>();
        if (gravitable) {
            gravitable.planet = planet;
        }
        GameObject shotgun = respawner.CreateObjectOnPlanet(shotgunPrefab);
        ItemService itemService = shotgun.GetComponent<ItemService>();
        if (!itemService) {
            return;
        }
        itemService.itemType = ItemType.WEAPON;
        itemService.weapon = Weapons.SHOTGUN;
        GameObject aura = respawner.CreateObjectOnPlanet(itemAuraPrefab, shotgun.transform.position);
        aura.transform.parent = shotgun.transform;
        aura.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
    }

    void CreateMedikit() {
        Gravitable gravitable = medikitPrefab.GetComponent<Gravitable>();
        if (gravitable) {
            gravitable.planet = planet;
        }
        GameObject medikit = respawner.CreateObjectOnPlanet(medikitPrefab);
        ItemService itemService = medikit.GetComponent<ItemService>();
        if (!itemService) {
            return;
        }
        itemService.itemType = ItemType.MEDIKIT;
        GameObject aura = respawner.CreateObjectOnPlanet(itemAuraPrefab, medikit.transform.position);
        aura.transform.parent = medikit.transform;
        aura.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
    }

    void CreateShield() {
        Gravitable gravitable = shieldPrefab.GetComponent<Gravitable>();
        if (gravitable) {
            gravitable.planet = planet;
        }
        GameObject shield = respawner.CreateObjectOnPlanet(shieldPrefab);
        ItemService itemService = shield.GetComponent<ItemService>();
        if (!itemService) {
            return;
        }
        itemService.itemType = ItemType.SHIELD;
        GameObject aura = respawner.CreateObjectOnPlanet(itemAuraPrefab, shield.transform.position);
        aura.transform.parent = shield.transform;
        aura.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
    }

    void CreateGrenade() {
        Gravitable gravitable = grenadePrefab.GetComponent<Gravitable>();
        if (gravitable) {
            gravitable.planet = planet;
        }
        GameObject grenade = respawner.CreateObjectOnPlanet(grenadePrefab);
        ItemService itemService = grenade.GetComponent<ItemService>();
        if (!itemService) {
            return;
        }
        itemService.itemType = ItemType.GRENADE;
        GameObject aura = respawner.CreateObjectOnPlanet(itemAuraPrefab, grenade.transform.position);
        aura.transform.parent = grenade.transform;
        aura.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
    }

    void CreateClock() {
        Gravitable gravitable = clockPrefab.GetComponent<Gravitable>();
        if (gravitable) {
            gravitable.planet = planet;
        }
        GameObject clock = respawner.CreateObjectOnPlanet(clockPrefab);
        ItemService itemService = clock.GetComponent<ItemService>();
        if (!itemService) {
            return;
        }
        itemService.itemType = ItemType.CLOCK;
        GameObject aura = respawner.CreateObjectOnPlanet(itemAuraPrefab, clock.transform.position);
        aura.transform.parent = clock.transform;
        aura.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
    }

    EventType GetRandomEventType() {
        return (EventType) Random.Range(0, System.Enum.GetValues(typeof(EventType)).Length);
    }

    enum EventType {
        NEW_SHOTGUN_EVENT,
        NEW_MEDIKIT_EVENT,
        NEW_SHIELD_EVENT,
        NEW_GRENADE_EVENT,
        NEW_CLOCK_EVENT,
    }

}
