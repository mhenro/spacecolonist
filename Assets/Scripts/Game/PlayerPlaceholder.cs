using UnityEngine;

public class PlayerPlaceholder : MonoBehaviour
{

    public GameObject player;
    public GameObject planet;

    // Update is called once per frame
    void Update() {
        //position
        transform.position = Vector3.Lerp(transform.position, player.transform.position + player.transform.up * 0.6f, 0.1f);
        Vector3 gravDirection = (transform.position - planet.transform.position).normalized;

        //rotation
        Quaternion toRotation = Quaternion.FromToRotation(transform.up, gravDirection) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 0.1f);
        //Debug.DrawLine(transform.position, transform.position + transform.up * 5f, Color.yellow);
    }

    public void NewPlanet(GameObject planet) {
        this.planet = planet;
    }

}
