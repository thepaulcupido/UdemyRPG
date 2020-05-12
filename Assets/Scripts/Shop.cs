using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public static Shop instance;

    public GameObject buyMenu;
    public GameObject sellMenu;
    public GameObject shopMenu;

    public Text goldText;
    public string[] itemsForSale = new string[40];
    public ItemButton[] buyItemButtons;
    public ItemButton[] sellItemButtons;

    void Start()
    {
        instance = this;       
    }
    
    public void OpenBuyMenu()
    {
        buyMenu.SetActive(true);
        sellMenu.SetActive(false);

        for (int i = 0; i < buyItemButtons.Length; i ++) {
            buyItemButtons[i].buttonValue = i;

            if (itemsForSale.Length <= 0) {
               break; 
            }

            if (itemsForSale[i] != "") {
                buyItemButtons[i].buttonImage.gameObject.SetActive(true);
                buyItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(itemsForSale[i]).itemSprite;
                buyItemButtons[i].amountText.text = "";
            } else {
                buyItemButtons[i].buttonImage.gameObject.SetActive(false);
                buyItemButtons[i].amountText.text = "";
            }
        }

        GameManager.instance.shopActive = true;
    }

    public void OpenSellMenu()
    {
        buyMenu.SetActive(false);
        sellMenu.SetActive(true);

        for (int i = 0; i < sellItemButtons.Length; i++) {
            sellItemButtons[i].buttonValue = i;

            GameManager.instance.SortItems();

            if (GameManager.instance.itemsHeld[i] != "") {
                sellItemButtons[i].buttonImage.gameObject.SetActive(true);
                sellItemButtons[i].buttonImage.sprite = GameManager.instance.GetItemDetails(GameManager.instance.itemsHeld[i]).itemSprite;
                sellItemButtons[i].amountText.text = "";
            } else {
                sellItemButtons[i].buttonImage.gameObject.SetActive(false);
                sellItemButtons[i].amountText.text = "";
            }
        }


        GameManager.instance.shopActive = true;
    }

    public void OpenShop()
    {
        shopMenu.SetActive(true);
        OpenBuyMenu();
        GameManager.instance.shopActive = true;
        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void CloseShop()
    {
        this.buyMenu.SetActive(false);
        this.sellMenu.SetActive(false);
        this.shopMenu.SetActive(false);

        GameManager.instance.shopActive = false;
        // GameMenu.instance.CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K) && !shopMenu.active) {
            OpenShop();
        }
    }
}
