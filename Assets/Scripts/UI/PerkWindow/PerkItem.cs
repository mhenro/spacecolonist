using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PerkItem : MonoBehaviour {

    public PerkType perkType;
    public string perkDescription;
    public GameObject perkDesc;

    private PerkService perkService;
    private GameObject perkWindow;
    private TimeManager timeManager;

    private void Start() {
        perkService = GameObject.FindGameObjectWithTag("PerkService").GetComponent<PerkService>();
        perkWindow = GameObject.FindGameObjectWithTag("PerkWindow");
        timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
    }

    void Update() {
        ProcessPerkDescription();
    }

    void ProcessPerkDescription() {
        Text text = perkDesc.GetComponent<Text>();
        if (!text) {
            return;
        }
        text.text = perkDescription;
    }

    public void ChoosePerk() {
        perkService.ApplyPerkToPlayer(perkType);
        OnClosePerkWindow();
    }

    public void OnClosePerkWindow() {
        perkWindow.SetActive(false);
        timeManager.ResumeGame();
    }

}
