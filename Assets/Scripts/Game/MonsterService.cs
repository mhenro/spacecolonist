using UnityEngine;
using UnityEngine.UI;

public class MonsterService : MonoBehaviour
{
    public Animator animator;
    public GameObject healthBar;

    private Rigidbody rb;
    private Gravitable gravitable;
    private GameObject player;
    private PlayerInventory inventory;
    private float nextDamageTime = 0f;
    private bool deadProcessed = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        gravitable = GetComponent<Gravitable>();

        player = GameObject.FindGameObjectWithTag("Player");

        inventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveToPlayer();
        ProcessHealth();
        ProcessDie();
    }

    void MoveToPlayer() {
        if (!player || inventory.died) {
            return;
        }
        Vector3 playerPos = player.transform.position;
        Vector3 playerDir = (playerPos - transform.position).normalized;

        //movement
        if (DistToPlayer(playerPos, transform.position) > 0.13f) {
            transform.position += transform.forward * inventory.speed * Time.deltaTime;
            animator.Play("Run");
        } else {
            animator.Play("Attack1");
            ProcessAttack();
        }

        //rotation
        Vector3 forward = Vector3.ProjectOnPlane(playerDir, transform.up);
        //Debug.DrawRay(transform.position, transform.forward * 100f, Color.red);
        transform.rotation = Quaternion.LookRotation(forward, transform.up);
        //Debug.Log("Distance to player = " + DistToPlayer(playerPos, transform.position));
    }

    void ProcessAttack() {
        if (Time.time <= nextDamageTime) {
            return;
        }
        DamageService.ApplyDamage(inventory, player.GetComponent<PlayerInventory>());
        FindObjectOfType<AudioManager>().Play("ClawsFire");
        FindObjectOfType<AudioManager>().Play("PlayerInjury");
        nextDamageTime = Time.time + Random.Range(0f, 10f);
    }

    private float DistToPlayer(Vector3 position1, Vector3 position2) {
        return Vector3.Distance(position1, position2);
        //return Mathf.Acos(Vector3.Dot(position1, position2));
    }

    void ProcessHealth() {
        Slider slider = healthBar.GetComponent<Slider>();
        if (!slider) {
            return;
        }
        if (inventory.maxHealth == inventory.health) {
            slider.gameObject.SetActive(false);
            return;
        }
        slider.gameObject.SetActive(true);
        slider.value = CalcHealthValue();
    }

    private float CalcHealthValue() {
        return (float) inventory.health / inventory.maxHealth;
    }

    void ProcessDie() {
        if (!inventory.died) {
            return;
        }
        animator.Play("Dead");
        Destroy(gameObject, 100f);
        gravitable.enabled = false;
        rb.detectCollisions = false;
        if (!deadProcessed) {
            deadProcessed = true;
            ++GameService.kills;
            GameService.currentExp += inventory.expForDead;
            FindObjectOfType<AudioManager>().Play("MonsterDead");
        }
    }

}

public enum MonsterType {
    ARACHNID,
    SLUG,
}
