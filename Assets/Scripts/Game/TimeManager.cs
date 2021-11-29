using UnityEngine;

public class TimeManager : MonoBehaviour {

    public float slowdownFactor = 0.05f;
    public float slowdownLength = 5f;

    private bool pause = false;

    // Update is called once per frame
    void Update() {
        if (!pause) {
            Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
    }

    public void DoSlowmotion() {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    public void PauseGame() {
        pause = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame() {
        Time.timeScale = 1f;
        pause = false;
    }

}
