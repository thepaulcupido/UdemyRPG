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
    }

    // Update is called once per frame
    void Update()
    {
        if (gameMenuOpen || isDialogActive || inSceneTransition) {
            PlayerController.instance.movementEnabled = false;
        } else {
            PlayerController.instance.movementEnabled = true;
        }

        //PlayerController.instance.movementEnabled = !(gameMenuOpen || isDialogActive || inSceneTransition);
    }

    public Item GetItemDetails(string itemName)
    {

        for (int i = 0; i < referenceItem.Length; i ++) {
            print(referenceItem[i].itemName + " " + itemName);
            if (referenceItem[i].itemName == itemName) {
                return referenceItem[i];
            }
        }

        return null;
    }
}
