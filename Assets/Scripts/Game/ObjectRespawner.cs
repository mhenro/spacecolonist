using UnityEngine;

public class ObjectRespawner : MonoBehaviour {

    private GameObject player;
    private GameObject planet;
    private float planetRadius;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        planet = GameObject.FindGameObjectWithTag("Planet");
        Mesh mesh = planet.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        planetRadius = Vector3.Distance(planet.transform.position, vertices[0]);
    }

    public GameObject CreateObjectOnPlanet(GameObject prefab, RespawnType respawnType) {
        switch (respawnType) {
            case RespawnType.OVER_THE_HORIZON: {
                Vector3 tempPos = player.transform.up /** planetRadius*/ * -3f;
                Vector3 spawnPosition = tempPos /** planetRadius*/ + planet.transform.position;
                return CreateObjectOnPlanet(prefab, spawnPosition);
            }
            case RespawnType.NEAR_THE_PLAYER: {
                Vector3 tempPos = player.transform.up /** planetRadius*/ * -3f;
                Vector3 spawnPosition = tempPos /** planetRadius*/ + planet.transform.position;
                return CreateObjectOnPlanet(prefab, spawnPosition);
            }
            default: {
                return CreateObjectOnPlanet(prefab);
            }
        }
    }

    public GameObject CreateObjectOnPlanet(GameObject prefab) {
        Vector3 spawnPosition = Random.onUnitSphere /** planetRadius*/ + planet.transform.position;
        return CreateObjectOnPlanet(prefab, spawnPosition);
    }

    public GameObject CreateObjectOnPlanet(GameObject prefab, Vector3 spawnPosition) {
        Vector3 gravDirection = (spawnPosition - planet.transform.position).normalized;
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity);
        obj.transform.rotation = Quaternion.FromToRotation(obj.transform.up, gravDirection);
        return obj;
    }

}

public enum RespawnType {
    RANDOM,
    OVER_THE_HORIZON,
    NEAR_THE_PLAYER,
}
