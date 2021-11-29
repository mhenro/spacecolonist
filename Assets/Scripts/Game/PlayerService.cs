using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerService : MonoBehaviour
{

    public Joystick joystick;
    public Animator animator;
    public GameObject playerModel;
    public GameObject planet;
    public ParticleSystem muzzleFlash;

    private Rigidbody rb;
    private PlayerInventory inventory;
    private LineRenderer laserSightLine;
    private GameObject shield;
    private bool freezeRotation;
    private float freezeRotationTime;

    private float armorTimer = 0.0f;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        inventory = GetComponent<PlayerInventory>();

        laserSightLine = GetComponent<LineRenderer>();
        laserSightLine.enabled = false;
        laserSightLine.useWorldSpace = true;
        laserSightLine.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        shield = GameObject.FindGameObjectWithTag("Shield");
    }

    // Update is called once per frame
    void Update() {
        ProcessMovement();
        ProcessRotation();
        ProcessInventory();
        if (Input.GetKeyDown(KeyCode.R)) {
            ProcessFire();
        }
        ProcessFreezeRotation();
    }

    void ProcessMovement() {
        Vector3 movementDir = new Vector3(joystick.Horizontal, 0f, joystick.Vertical).normalized;
        animator.SetFloat("Speed", (Mathf.Abs(joystick.Vertical) + Mathf.Abs(joystick.Horizontal)) > 0f ? 1f : 0f);
        transform.Translate(movementDir * inventory.speed * Time.deltaTime);
        if (Mathf.Abs(joystick.Vertical) + Mathf.Abs(joystick.Horizontal) > 0) {
            FindObjectOfType<AudioManager>().PlayOnce("Running");
        } else {
            FindObjectOfType<AudioManager>().Stop("Running");
        }
    }

    void ProcessRotation() {
        Vector3 dir = (transform.position - planet.transform.position).normalized;
        Vector3 forward = Vector3.ProjectOnPlane(dir, transform.up);
        if (joystick.Horizontal != 0 && joystick.Vertical != 0 && !freezeRotation) {
            playerModel.transform.rotation = Quaternion.LookRotation(forward.normalized, transform.up);
        }
    }

    void ProcessInventory() {
        ProcessLaserSight();
        ProcessShield();
        ProcessHealth();
        ProcessAmmo();
        ProcessLevel();
    }

    void ProcessLaserSight() {
        if (!inventory.laserSight) {
            laserSightLine.enabled = false;
            return;
        }
        laserSightLine.enabled = true;
        laserSightLine.SetPosition(0, playerModel.transform.position + playerModel.transform.up * -0.45f);
        laserSightLine.material.color = Color.red;
        RaycastHit aimHit;
        if (!Physics.Raycast(playerModel.transform.position + playerModel.transform.up * -0.45f, playerModel.transform.forward, out aimHit, 10f) || !aimHit.collider) {
            laserSightLine.SetPosition(1, playerModel.transform.position + playerModel.transform.forward * 10f);
        } else {
            laserSightLine.SetPosition(1, aimHit.point);
        }
    }

    void ProcessShield() {
        if (!inventory.shield) {
            shield.SetActive(false);
            return;
        }
        shield.SetActive(true);
        armorTimer += Time.deltaTime;
        if (inventory.armorPersistSec != -1 && (armorTimer % 60) >= inventory.armorPersistSec) {
            inventory.shield = false;
        }
    }

    void ProcessHealth() {
        Text healthValue = GameObject.FindGameObjectWithTag("HealthValue").GetComponent<Text>();
        healthValue.text = $"{inventory.health}/{inventory.maxHealth}";
        GameObject healthSlider = GameObject.FindGameObjectWithTag("HealthSlider");
        if (!healthSlider) {
            return;
        }
        Slider slider = healthSlider.GetComponent<Slider>();
        float health = CalcHealthValue();
        slider.value = health;
        if (health <= 0 || inventory.died) {
            Die();
        }
    }

    float CalcHealthValue() {
        return (float) inventory.health / inventory.maxHealth;
    }

    void Die() {
        FindObjectOfType<AudioManager>().Play("PlayerDead");
        GameService.endTime = Time.time;
        CustomSceneManager.SwitchToGameOver();
    }

    void ProcessAmmo() {
        ProcessAmmoDescription();
        ProcessAmmoSlider();
        ProcessAmmoImage();
    }

    void ProcessAmmoDescription() {
        GameObject weaponTip = GameObject.FindGameObjectWithTag("WeaponTip");
        Text weaponTipText = weaponTip.GetComponent<Text>();
        weaponTipText.text = CalcAmmoDescription();

        Text ammoValue = GameObject.FindGameObjectWithTag("AmmoValue").GetComponent<Text>();
        ammoValue.text = CalcAmmoValueText();
    }

    void ProcessAmmoSlider() {
        GameObject ammoSlider = GameObject.FindGameObjectWithTag("AmmoSlider");
        if (!ammoSlider) {
            return;
        }
        Slider slider = ammoSlider.GetComponent<Slider>();
        slider.value = CalcAmmoValue();
    }

    void ProcessAmmoImage() {
        GameObject weaponImage = GameObject.FindGameObjectWithTag("WeaponImage");
        if (!weaponImage) {
            return;
        }
        Image image = weaponImage.GetComponent<Image>();
        image.sprite = WeaponService.GetWeaponSprite(inventory.currentWeapon);
    }

    string CalcAmmoDescription() {
        WeaponProperties weaponProperties = WeaponService.GetWeaponProperties(inventory.currentWeapon);
        if (weaponProperties.BulletCount == -1) {
            return weaponProperties.Description + " ( \u221E )";
        }
        return weaponProperties.Description + " (" + inventory.bulletCount + " / " + weaponProperties.BulletCount + ")";
    }

    string CalcAmmoValueText() {
        WeaponProperties weaponProperties = WeaponService.GetWeaponProperties(inventory.currentWeapon);
        if (weaponProperties.BulletCount == -1) {
            return "\u221E/\u221E";
        }
        return $"{inventory.bulletCount}/{weaponProperties.BulletCount}";
    }

    float CalcAmmoValue() {
        WeaponProperties weaponProperties = WeaponService.GetWeaponProperties(inventory.currentWeapon);
        if (weaponProperties.BulletCount == -1) {
            return 1f;
        }
        return (float) inventory.bulletCount / weaponProperties.BulletCount;
    }

    void ProcessLevel() {
        GameObject levelTip = GameObject.FindGameObjectWithTag("LevelTip");
        TextMeshProUGUI text = levelTip.GetComponent<TextMeshProUGUI>();
        if (!text) {
            Debug.Log("null :(");
            return;
        }
        text.text = BuildLevelTip();
    }

    string BuildLevelTip() {
        return $"Level {GameService.level} (exp. {GameService.currentExp}/{GameService.levelUpExp})";
    }

    public void ProcessFire() {
        if ((inventory.bulletCount -= 1) < 0) {
            inventory.bulletCount = 0;
            inventory.currentWeapon = Weapons.PISTOL;
        }
        WeaponProperties weaponProperties = WeaponService.GetWeaponProperties(inventory.currentWeapon);
        if (weaponProperties.BulletCount != -1 && inventory.bulletCount <= 0) {
            return;
        }
        FindObjectOfType<AudioManager>().Play(weaponProperties.FireFx);
        ++GameService.shots;
        muzzleFlash.Play();
        freezeRotation = true;
        freezeRotationTime = Time.time + 0.3f;
        RaycastHit fireHit;
        //Debug.DrawRay(playerModel.transform.position + playerModel.transform.up * -0.45f, playerModel.transform.forward * 10f, Color.red);
        if (!Physics.Raycast(playerModel.transform.position + playerModel.transform.up * -0.45f, playerModel.transform.forward, out fireHit, weaponProperties.Distance) || !fireHit.collider) {
            return;
        }
        PlayerInventory monsterInventory = fireHit.transform.GetComponent<PlayerInventory>();
        if (!monsterInventory) {
            return;
        }
        DamageService.ApplyDamage(inventory, monsterInventory);
        /* מעהאקא
        if (fireHit.rigidbody && !monsterInventory.died) {
            fireHit.rigidbody.AddForce(-fireHit.normal * 10f);
        }
        */
    }

    void ProcessFreezeRotation() {
        if (Time.time <= freezeRotationTime) {
            return;
        }
        freezeRotation = false;
        freezeRotationTime = Time.time + 0.3f;
    }

    private void OnTriggerEnter(Collider other) {
        ItemService itemService = other.GetComponent<ItemService>();
        if (!itemService) {
            return;
        }
        switch (itemService.itemType) {
            case ItemType.WEAPON: {
                WeaponProperties weaponProperties = WeaponService.GetWeaponProperties(itemService.weapon);
                inventory.currentWeapon = itemService.weapon;
                inventory.bulletCount = weaponProperties.BulletCount;
                FindObjectOfType<AudioManager>().Play("EquipWeapon");
                break;
            }
            case ItemType.MEDIKIT: {
                inventory.health = inventory.maxHealth;
                FindObjectOfType<AudioManager>().Play("EquipHeal");
                break;
            }
            case ItemType.SHIELD: {
                inventory.shield = true;
                armorTimer = 0f;
                break;
            }
            case ItemType.GRENADE: {
                Collider[] monstersInRange = Physics.OverlapSphere(other.transform.position, 0.3f);
                foreach(Collider collider in monstersInRange) {
                    PlayerInventory monsterInventory = collider.GetComponent<PlayerInventory>();
                    if (!monsterInventory || monsterInventory.player) {
                        continue;
                    }
                    DamageService.ApplyDamage(monsterInventory, 100);
                }
                ParticleSystem effect = Instantiate(itemService.effectPrefab.GetComponent<ParticleSystem>() , other.transform.position, Quaternion.identity);
                FindObjectOfType<AudioManager>().Play("SmallExplosion");
                Destroy(effect.gameObject, 1f);
                break;
            }
            case ItemType.CLOCK: {
                TimeManager timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
                timeManager.DoSlowmotion();
                break;
            }
        }
        Destroy(other.gameObject);
    }

}
