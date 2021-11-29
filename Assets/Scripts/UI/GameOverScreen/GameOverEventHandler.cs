using UnityEngine;

public class GameOverEventHandler : MonoBehaviour
{
    
    public void OnBackButtonClick() {
        CustomSceneManager.SwitchToMainMenu();
    }

}
