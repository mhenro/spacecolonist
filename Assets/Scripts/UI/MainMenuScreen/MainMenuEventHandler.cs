using UnityEngine;

public class MainMenuEventHandler : MonoBehaviour {

    public void OnButtonStartClick() {
        CustomSceneManager.SwitchToGame();
    }

    public void OnButtonOptionsClick() {
        //CustomSceneManager.SwitchToOptions();
    }

    public void OnButtonRankingClick() {
        CustomSceneManager.SwitchToRanking();
    }

    public void OnButtonExitClick() {
        Application.Quit();
    }

}
