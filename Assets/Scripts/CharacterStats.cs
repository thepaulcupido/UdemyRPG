using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public string characterName;
    public int characterLevel;
    public int currentExp;
    public int[] expToNextLevel;
    public int baseExperience = 1000;
    public int maxLevel = 100;
    public int currentHP;
    public int maxHP = 100;
    public int currentMp;
    public int maxMP = 30;
    public int[] mpLevelBonus;
    public int strength;
    public int defence;
    public int weaponPower;
    public int armourPower;
    public int equippedWeapn;
    public int equippedArmour;

    public Sprite characterImage;

    // Start is called before the first frame update
    void Start()
    {
        expToNextLevel = new int[maxLevel];
        expToNextLevel[1] = baseExperience;
        
        for (int index = 2; index < maxLevel; index++) {
            expToNextLevel[index] = Mathf.FloorToInt(expToNextLevel[index-1] * 1.05f);
        }

    }

     void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            GainExp(1000);
        }
    }

    public void GainExp(int expToGain)
    {
        this.currentExp += expToGain;
        if (currentExp >= expToNextLevel[characterLevel]) {
            // then level up
            currentExp -= expToNextLevel[characterLevel];
            characterLevel++;

            if (characterLevel%2 != 0) {
                strength++;
            } else {
                defence ++;
            }
            
            maxHP = Mathf.FloorToInt(maxHP * 1.05f);
            currentHP = maxHP;
            maxMP += mpLevelBonus[characterLevel];
            currentMp = maxMP;
        }
    }


}
