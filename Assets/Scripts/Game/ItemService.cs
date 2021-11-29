using UnityEngine;

public class ItemService : MonoBehaviour
{
    public float rotationSpeed = 45f;
    public ItemType itemType;
    public Weapons weapon;
    public GameObject effectPrefab;

    private void FixedUpdate() {
        transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
    }

}

public enum ItemType {
    WEAPON,
    GRENADE,
    MEDIKIT,
    SHIELD,
    CLOCK,
}