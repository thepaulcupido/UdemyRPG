using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public CharacterStats[] playerStats;
    public bool gameMenuOpen, isDialogActive, inSceneTransition;

    public string[] itemsHeld;
    public int[] numberOfItems;
    public Item[] referenceItem;

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        SortItems();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || isDialogActive || inSceneTransition) {
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
}
