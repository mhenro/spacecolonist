using UnityEngine;

public class Gravitable : MonoBehaviour
{

    private const float DISTANCE_THRESHOLD = 0.01f;

    public GameObject planet;
    public float gravity = 9.8f;
    public bool debug = false;

    private Rigidbody rb;
    private float distanceToGround;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update() {
        //ground control
        bool onGround = false;
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10)) {
            distanceToGround = hit.distance;
            onGround = distanceToGround <= DISTANCE_THRESHOLD;
        }

        //gravity & rotation
        Vector3 gravDirection = (transform.position - planet.transform.position).normalized;
        if (!onGround) {
            rb.AddForce(gravDirection * -gravity);
        }
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
        transform.rotation = toRotation;

        if (debug) {
            Debug.DrawLine(transform.position, transform.position + transform.up * 5f, Color.yellow);
        }
    }
}
