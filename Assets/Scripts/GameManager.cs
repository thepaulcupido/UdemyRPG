using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public CharacterStats[] playerStats;
    public bool gameMenuOpen, isDialogActive, inSceneTransition, isBattleActive;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItem;
    public bool shopActive;

    public int currentGold = 0; 

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || isDialogActive || inSceneTransition || shopActive || isBattleActive) {
            PlayerController.instance.movementEnabled = false;
        } else {
            PlayerController.instance.movementEnabled = true;
        }

        if (Input.GetKeyUp(KeyCode.J)) {
            AddItem("Iron Armour");
        }

        if (Input.GetKeyUp(KeyCode.K)) {
            RemoveItem("Health Potion");
            RemoveItem("Leather Armour - Bleep");
        }

        if (Input.GetKeyUp(KeyCode.P)) {
            this.SaveData();
        }
        if (Input.GetKeyUp(KeyCode.O)) {
            this.LoadData();
        }

        //PlayerController.instance.movementEnabled = !(gameMenuOpen || isDialogActive || inSceneTransition);
    }

    public Item GetItemDetails(string itemName)
    {

        for (int i = 0; i < referenceItem.Length; i ++) {
            if (referenceItem[i].itemName == itemName) {
                return referenceItem[i];
            }
        }

        return null;
    }

    public void SortItems()
    {

        bool itemAfterSpace = true;

        while (itemAfterSpace) {
            itemAfterSpace = false;

            for (int i = 0; i < itemsHeld.Length -1; i ++) {
               if (itemsHeld[i] == "") {
                    itemsHeld[i] = itemsHeld[i+1];
                    itemsHeld[i+1] = "";

                    numberOfItems[i] = numberOfItems[i+1];
                    numberOfItems[i+1] = 0;

                    if (itemsHeld[i] != "") {
                        itemAfterSpace = true;
                    }
                }
            }
        }
    }

    public void AddItem(string itemToAdd)
    {
        bool found = false;
        int newItemPosition = -1;

        for (int i = 0; i < itemsHeld.Length; i ++)
        {
            if (itemToAdd == itemsHeld[i] && numberOfItems[i] < 999) {
                found = true;
                newItemPosition = i;
                break;
            } else if (itemsHeld[i] == "") {
                newItemPosition = i;
                break;
            }
        }

        // check item validity
        bool exists = CheckItemValidity(itemToAdd);

        if (exists) {
            if (newItemPosition != -1) {
                itemsHeld[newItemPosition] = itemToAdd;
                numberOfItems[newItemPosition]++;
            } else {
                Debug.Log("The inventory is currently full and cannot accomodate more of this item type. Please free up an item slot.");
            }   
        } else {
            Debug.Log("The item to be added (" + itemToAdd + ") does not exist");
        }
        
        GameMenu.instance.ShowItems();
    }

    public void RemoveItem(string itemToRemove)
    {
        bool found = false;
        int itemPosition = -1;

        for (int i = 0; i < itemsHeld.Length; i ++)
        {
            if (itemToRemove == itemsHeld[i]) {
                found = true;
                itemPosition = i;
                break;
            }
        }

        bool exists = CheckItemValidity(itemToRemove);

        if (exists) {
            if (itemPosition != -1) {
                itemsHeld[itemPosition] = itemToRemove;
                numberOfItems[itemPosition]--;

                if (numberOfItems[itemPosition] <= 0) {
                    itemsHeld[itemPosition] = "";
                }
            } 
        } else {
            Debug.Log("The item to be removed (" + itemToRemove + ") does not exist");
        }

        GameMenu.instance.ShowItems();
    }

    private bool CheckItemValidity(string itemName)
    {
        bool exists = false;
        for (int i = 0; i < referenceItem.Length; i++) {
            if (referenceItem[i].itemName == itemName) {
                exists = true;
                break;
            }
        }

        return exists;
    }

    public void SaveData()
    {
        // save current scene
        PlayerPrefs.SetString("Current_Scene",SceneManager.GetActiveScene().name);
        // where the player is standing
        PlayerPrefs.SetFloat("Player_Position_x", PlayerController.instance.transform.position.x);
        PlayerPrefs.SetFloat("Player_Position_y", PlayerController.instance.transform.position.y);
        PlayerPrefs.SetFloat("Player_Position_z", PlayerController.instance.transform.position.z);

        // Save Player information
        for (int i = 0 ; i < playerStats.Length; i ++) {
            if (playerStats[i].gameObject.active) {
                PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_active", 1);
            } else {
                PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_active", 0);
            }

            PlayerPrefs.SetString("Player_" + playerStats[i] + "_name", playerStats[i].characterName);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_level", playerStats[i].characterLevel);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_exp", playerStats[i].currentExp);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_health", playerStats[i].currentHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_mana", playerStats[i].currentMp);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_max-health", playerStats[i].maxHP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_max-mana", playerStats[i].maxMP);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_strength", playerStats[i].strength);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_defence", playerStats[i].defence);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_weapon-power", playerStats[i].weaponPower);
            PlayerPrefs.SetInt("Player_" + playerStats[i].characterName + "_armour-power", playerStats[i].armourPower);
            PlayerPrefs.SetString("Player_" + playerStats[i].characterName + "_equipped-weapon", playerStats[i].equippedWeapn);
            PlayerPrefs.SetString("Player_" + playerStats[i].characterName + "_equipped-armour", playerStats[i].equippedArmour);
        }

        // Save player inventory data
       for (int j = 0; j < itemsHeld.Length; j++) {
           PlayerPrefs.SetString("ItemInInventory_" + j, itemsHeld[j]);
           PlayerPrefs.SetInt("ItemAmount_" + j, numberOfItems[j]);
       }
        
    }

    public void LoadData()
    {
        float x = PlayerPrefs.GetFloat("Player_Position_x");
        float y = PlayerPrefs.GetFloat("Player_Position_y");
        float z = PlayerPrefs.GetFloat("Player_Position_z");

        PlayerController.instance.transform.position = new Vector3(x, y , z);

        for (int i = 0 ; i < playerStats.Length; i ++) {
            if (PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_active") == 0) {
                playerStats[i].gameObject.SetActive(false);
            } else {
                playerStats[i].gameObject.SetActive(true);
            }

            // Load all player stats
            playerStats[i].characterName = PlayerPrefs.GetString("Player_" + playerStats[i] + "_name");
            playerStats[i].characterLevel = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_level");
            playerStats[i].currentExp = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_exp");
            playerStats[i].currentHP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_health");
            playerStats[i].currentMp = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_mana");
            playerStats[i].maxHP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_max-health");
            playerStats[i].maxMP = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_max-mana");
            playerStats[i].strength = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_strength");
            playerStats[i].defence = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_defence");
            playerStats[i].weaponPower = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_weapon-power");
            playerStats[i].armourPower = PlayerPrefs.GetInt("Player_" + playerStats[i].characterName + "_armour-power");
            playerStats[i].equippedWeapn = PlayerPrefs.GetString("Player_" + playerStats[i].characterName + "_equipped-weapon");
            playerStats[i].equippedArmour = PlayerPrefs.GetString("Player_" + playerStats[i].characterName + "_equipped-armour");
        }

        // Load all items in inventory
        for (int j = 0; j < itemsHeld.Length; j++) {
           itemsHeld[j] = PlayerPrefs.GetString("ItemInInventory_" + j);
           numberOfItems[j] = PlayerPrefs.GetInt("ItemAmount_" + j);
       }
    }
}
