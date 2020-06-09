using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleMagicSelection : MonoBehaviour
{

    public string spellName;
    public int manaCost;
    public Text nameText;
    public Text costText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Press()
    {
        int currentActor = BattleManager.instance.currentTurn;

        if (BattleManager.instance.activeBattlers[currentActor].currentMp >= manaCost) {
            BattleManager.instance.magicMenu.SetActive(false);
            BattleManager.instance.OpenTargetMenu(spellName);
            BattleManager.instance.activeBattlers[currentActor].currentMp -= manaCost;
        } else {
            BattleManager.instance.notification.description.text = "Not Enough MP!";
            BattleManager.instance.notification.Activate();
            BattleManager.instance.magicMenu.SetActive(false);
        }
        
    }

}
