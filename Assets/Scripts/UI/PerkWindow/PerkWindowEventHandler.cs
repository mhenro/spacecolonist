using UnityEngine;
using UnityEngine.UI;

public class PerkWindowEventHandler : MonoBehaviour {

    public PerkService perkService;
    public GameObject perk1Image;
    public GameObject perk1Desc;
    public GameObject perkDescription;

    public void SetPerkDescription(int perkIndex) {
        Text text = perkDescription.GetComponent<Text>();
        if (!text) {
            return;
        }
        switch (perkIndex) {
            case 1: text.text = "Perk 1"; break;
            case 2: text.text = "Perk 2"; break;
            case 3: text.text = "Perk 3"; break;
        }
    }

}
