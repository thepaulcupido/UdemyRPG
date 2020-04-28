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
}
