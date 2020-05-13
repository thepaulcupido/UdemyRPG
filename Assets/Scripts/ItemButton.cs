using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{

    public Image buttonImage;
    public Text amountText;
    public int buttonValue;
    
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
        string itemName = GameManager.instance.itemsHeld[buttonValue];
        Item item;

        if (GameMenu.instance.gameMenu.active) {
            if (itemName != "") {
                item = GameManager.instance.GetItemDetails(itemName);
                GameMenu.instance.SelectItem(item);
            }
        }
        
        if (Shop.instance.shopMenu.active) {
            if (Shop.instance.buyMenu.active) {
                itemName = Shop.instance.itemsForSale[buttonValue];

                if (itemName != "") {
                    item = GameManager.instance.GetItemDetails(itemName);
                    Shop.instance.SelectBuyItem(item);
                }
                
            }  
            if (Shop.instance.sellMenu.active) {
                
                if (itemName != "") {
                    item = GameManager.instance.GetItemDetails(itemName);
                    Shop.instance.SelectSellItem(item);
                }
            }
        }
    }
}
