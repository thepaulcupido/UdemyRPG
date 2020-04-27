using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    private CharacterStats[] playerStats;

    public GameObject gameMenu;
    public Text[] nameText, hpText, mpText, levelText, expNextLevelText;
    public Slider[] expSlider;
    public Image[] characterImage;
    public GameObject[] characterStatHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2")) {

            if (this.gameMenu.active) {
                gameMenu.SetActive(false);
                GameManager.instance.gameMenuOpen = false;
            } else {
                gameMenu.SetActive(true);
                GameManager.instance.gameMenuOpen = true;
                this.UpdateMainStats();
            }
        }
    }

    public void UpdateMainStats()
    {
        this.playerStats = GameManager.instance.playerStats;

        for (int index = 0; index < playerStats.Length; index++) {
            if (playerStats[index].gameObject.active) {
                characterStatHolder[index].SetActive(true);

                // Update player stats
                nameText[index].text = playerStats[index].characterName;
                hpText[index].text = "HP: " + playerStats[index].currentHP + "/" + playerStats[index].maxHP;
                mpText[index].text = "MP: " + playerStats[index].currentMp + "/" + playerStats[index].maxMP;
                
                levelText[index].text = "Level: " + playerStats[index].characterLevel;
                expNextLevelText[index].text = "" + playerStats[index].currentExp + "/" + playerStats[index].expToNextLevel[playerStats[index].characterLevel];
                expSlider[index].maxValue = playerStats[index].expToNextLevel[playerStats[index].characterLevel];
                expSlider[index].value = playerStats[index].currentExp;

                characterImage[index].sprite = playerStats[index].characterImage;

            } else {
                characterStatHolder[index].SetActive(false);
                break;
            }
        }
    }
}
