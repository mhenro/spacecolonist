using UnityEngine.SceneManagement;

public static class CustomSceneManager {
    
    public static void SwitchToMainMenu() {
        SceneManager.LoadScene("MainMenuScene");
    }

    public static void SwitchToGame() {
        SceneManager.LoadScene("GameScene");
    }

    public static void SwitchToGameOver() {
        SceneManager.LoadScene("GameOverScene");
    }

    public static void SwitchToRanking() {
        SceneManager.LoadScene("RankingScene");
    }

}
