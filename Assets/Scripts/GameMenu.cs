﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    //private variables
    private CharacterStats[] playerStats;

    // public variables
    public GameObject gameMenu;
    public Text[] nameText, hpText, mpText, levelText, expNextLevelText;
    public Text statusName, statusHP, statusMP, statusStrength, statusDefence, statusWpnEquip, statusWpnPower, statusArmourEquip, statusAmourPower, statusExp;
    public Image statusImage;
    public Slider[] expSlider;
    public Image[] characterImage;
    public GameObject[] characterStatHolder;
    public GameObject[] windows;
    public GameObject[] statusButtons;
    public ItemButton[] itemButtons;
    public string selectedItem;
    public Item activeItem;
    public Text itemName, itemDescription, useButtonText;
    public Text[] itemCharChoiceNames;
    public GameObject SideButtonMenu;
    public GameObject CharacterInfoMenu;
    public GameObject itemCharacterChoiceMenu;
    public static GameMenu instance;

    public Text goldText;
    public string mainMenuName;

    void Start()
    {
        this.UpdateMainStats();
        instance = this;
    }


    void Update()
    {
        if (Input.GetButtonDown("Fire2")) {

            if (this.gameMenu.active) {
                this.CloseMenu();
                
            } else {
                
                this.OpenMenu();
                
                GameManager.instance.gameMenuOpen = true;
                this.UpdateMainStats();
            }
        }
    }

    public void UpdateCharacterStatus(int x)
    {
        if (x < playerStats.Length) {
            statusName.text = "" + playerStats[x].characterName;
            statusHP.text = "" + playerStats[x].currentHP + "/" + playerStats[x].maxHP;
            statusMP.text = "" + playerStats[x].currentMp + "/" + playerStats[x].maxMP;
            statusStrength.text = playerStats[x].strength.ToString();
            statusDefence.text = playerStats[x].defence.ToString();

            statusWpnPower.text = playerStats[x].weaponPower.ToString();
            statusAmourPower.text = playerStats[x].armourPower.ToString();

            statusWpnEquip.text = (playerStats[x].equippedWeapn != "") ? playerStats[x].equippedWeapn.ToString() : "None";
            statusArmourEquip.text = (playerStats[x].equippedArmour != "") ? playerStats[x].equippedArmour.ToString() : "None";

            statusExp.text = (playerStats[x].expToNextLevel[playerStats[x].characterLevel] - playerStats[x].currentExp).ToString();
            statusImage.sprite = playerStats[x].characterImage;
        }
    }

    public void ToggleWindow(int index)
    {
        for (int x = 0; x < windows.Length; x++) {
            if (index == x) {
                windows[x].SetActive(!windows[x].active);
            } else {
                windows[x].SetActive(false);
            }
        }
        this.itemCharacterChoiceMenu.SetActive(false);
    }

    public void CloseMenu()
    {
        for (int x = 0; x < windows.Length; x++) {
            if (windows[x].active) {
                windows[x].SetActive(false);
            }
        }

        Shop.instance.CloseShop();
        
        gameMenu.SetActive(false);
        
        this.itemCharacterChoiceMenu.SetActive(false);
        GameManager.instance.gameMenuOpen = false;

        AudioManager.instance.PlaySFX(5);
    }

    public void OpenMenu()
    {
        SideButtonMenu.SetActive(true);
        CharacterInfoMenu.SetActive(true);
        
        gameMenu.SetActive(true);
        
        this.itemCharacterChoiceMenu.SetActive(true);
        GameManager.instance.gameMenuOpen = true;
    }

    public void OpenStatus()
    {
        // update information
        UpdateCharacterStatus(0);

        // show buttons
        for (int x = 0; x < statusButtons.Length; x ++) {
            statusButtons[x].SetActive(playerStats[x].gameObject.active);
            statusButtons[x].GetComponentInChildren<Text>().text = playerStats[x].characterName;
        }
    }

    public void UpdateMainStats()
    {
        if (GameManager.instance != null) {
            this.playerStats = GameManager.instance.playerStats;
            goldText.text = GameManager.instance.currentGold.ToString() + "g";

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

    public void ShowItems()
    {
        GameManager.instance.SortItems();

        for (int i = 0; i < itemButtons.Length; i++) {
            itemButtons[i].buttonValue = i;

            //GameManager.instance.itemsHeld[i];

            if (GameManager.instance.itemsHeld[i] != "") {
                itemButtons[i].buttonImage.gameObject.SetActive(true);
                itemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                itemButtons[i].amountText.text = "";
            } else {
                itemButtons[i].buttonImage.gameObject.SetActive(false);
                itemButtons[i].amountText.text = "";
            }
        }
    }

    public void SelectItem(Item selectedItem)
    {
        activeItem = selectedItem;
        
        if (activeItem.isItem) {
            useButtonText.text = "Use";
        } else if (activeItem.isWeapon || activeItem.isArmour) {
            useButtonText.text = "Equip";
        }

        itemName.text = activeItem.itemName;
        itemDescription.text = activeItem.description;
    }

    public void DiscardItem()
    {
        if (activeItem != null) {
            GameManager.instance.RemoveItem(activeItem.itemName);
        }
    }

    public void OpenItemCharacterChoice()
    {
        CharacterStats stats;
        this.itemCharacterChoiceMenu.SetActive(true);

        for (int i = 0; i < itemCharChoiceNames.Length; i++) {
            stats = GameManager.instance.playerStats[i];

            itemCharChoiceNames[i].text = stats.characterName;
            itemCharChoiceNames[i].transform.parent.gameObject.SetActive(stats.gameObject.active);
        }
    }

    public void CloseItemCharacterChoice()
    {
        this.itemCharacterChoiceMenu.SetActive(false);
    }

    public void UseItem(int selectChar)
    {
        activeItem.Use(selectChar);
        UpdateMainStats();
        CloseItemCharacterChoice();
    }

    public void SaveGame()
    {
        GameManager.instance.SaveData();
        QuestManager.instance.SaveQuestData();
    }

    public void PlayButtonSound()
    {
        // todo: replace integers with values from a Config file or a enumerated variables
        AudioManager.instance.PlaySFX(4);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene(mainMenuName);
        Destroy(GameManager.instance.gameObject);
        Destroy(PlayerController.instance.gameObject);
        Destroy(AudioManager.instance.gameObject);
        Destroy(gameObject);
    }

}
