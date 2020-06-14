using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [Header("Item Type")]
    public bool isItem;
    public bool isWeapon;
    public bool isArmour;
    
    [Header("Item Details")]
    public string itemName;
    public string description;
    public int value;
    public Sprite itemSprite;

    [Header("Item Details")]
    public int itemPower;    // rename of amounToChange
    public bool affectsHp, affectsMp, affectsStr;
    [Header("Weapon/ Armour Details")]
    public int weaponStr;
    public int armourStr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Use(int character) {
        CharacterStats selectedChar = GameManager.instance.playerStats[character];
        
        // I could avoid some serious code dulpication here if I simply use polymorphism such that this function can apply to both BattleCharacters and CharacterStats
        // This would be better using enumerables and a switch statement
        if (isItem) {
            if (affectsHp) {
                selectedChar.currentHP += itemPower;
                if (selectedChar.currentHP > selectedChar.maxHP) {
                    selectedChar.currentHP = selectedChar.maxHP;
                }
            } else if (affectsMp) {
                selectedChar.currentMp += itemPower;
                if (selectedChar.currentMp > selectedChar.maxMP) {
                    selectedChar.currentMp = selectedChar.maxMP;
                }
            } else if (affectsStr) {
                selectedChar.strength += itemPower;
            }
        }

        if (isWeapon) {
            if (selectedChar.equippedWeapn != "" && selectedChar.equippedWeapn != "0") {
                GameManager.instance.AddItem(selectedChar.equippedWeapn);
            }

            selectedChar.equippedWeapn = itemName;
            selectedChar.weaponPower = weaponStr;
        }

        if (isArmour) {
            if (selectedChar.equippedArmour != "" && selectedChar.equippedArmour != "0") {
                GameManager.instance.AddItem(selectedChar.equippedArmour);
            }

            selectedChar.equippedArmour = itemName;
            selectedChar.armourPower = armourStr;
        }

        GameManager.instance.playerStats[character] = selectedChar;
        GameManager.instance.RemoveItem(itemName);
    }

    public void Use(BattleCharacter selectedChar)
    {
        int index = -1;
        for (int i = 0; i < BattleManager.instance.activeBattlers.Count; i ++) {
            if (BattleManager.instance.activeBattlers[i].characterName == selectedChar.characterName) {
                index = i;
                break;
            }
        }

        if (isItem) {
            if (affectsHp) {
                selectedChar.currentHp += itemPower;
                if (selectedChar.currentHp > selectedChar.maxHp) {
                    selectedChar.currentHp = selectedChar.maxHp;
                }
            } else if (affectsMp) {
                selectedChar.currentMp += itemPower;
                if (selectedChar.currentMp > selectedChar.maxMp) {
                    selectedChar.currentMp = selectedChar.maxMp;
                }
            } else if (affectsStr) {
                selectedChar.strength += itemPower;
            }
        }

        if (isWeapon) {
            // if (selectedChar.equippedWeapn != "" && selectedChar.equippedWeapn != "0") {
            //     GameManager.instance.AddItem(selectedChar.equippedWeapn);
            // }

            // selectedChar.equippedWeapn = itemName;
            selectedChar.weaponPower = weaponStr;
        }

        if (isArmour) {
            // if (selectedChar.equippedArmour != "" && selectedChar.equippedArmour != "0") {
            //     GameManager.instance.AddItem(selectedChar.equippedArmour);
            // }

            // selectedChar.equippedArmour = itemName;
            selectedChar.armourPower = armourStr;
        }

        // GameManager.instance.playerStats[character] = selectedChar;

        if (index > -1) {
            BattleManager.instance.activeBattlers[index] = selectedChar;
            BattleManager.instance.UpdateUIStats();
            GameManager.instance.RemoveItem(itemName);
        }
    }
}
