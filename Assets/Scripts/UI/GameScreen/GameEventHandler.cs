using UnityEngine;

public class GameEventHandler : MonoBehaviour
{

    private GameObject player;

    public void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void OnFireButtonClick() {
        PlayerService playerService = player.GetComponent<PlayerService>();
        playerService.ProcessFire();
    }

}
