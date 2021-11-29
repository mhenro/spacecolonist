using UnityEngine;
using UnityEngine.UI;

public class LoadingSlider : MonoBehaviour {

    public float speed = 0.5f;

    private Slider slider;
    private float time = 0f;
    private bool loading = true;

    // Start is called before the first frame update
    void Start() {
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update() {
        if (loading) {
            time += Time.deltaTime * speed;
            slider.value = time;
        }
        if (time > 1) {
            loading = false;
            time = 0f;
            CustomSceneManager.SwitchToMainMenu();
        }
    }

}
