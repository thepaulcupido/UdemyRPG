﻿using System.Collections;
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

    public Item selectedItem;
    public Text buyItemName, buyItemDescription, buyItemValue;
    public Text sellItemName, sellItemDescription, sellItemValue;

    void Start()
    {
        instance = this;       
    }
    
    public void OpenBuyMenu()
    {
        buyItemButtons[0].Press();

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
        sellItemButtons[0].Press();

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

    public void SelectBuyItem(Item buyItem)
    {
        selectedItem = buyItem;
    
        buyItemName.text = selectedItem.itemName;
        buyItemDescription.text = selectedItem.description;
        buyItemValue.text = "Value: " + selectedItem.value + "g";
    }

    public void SelectSellItem(Item sellItem)
    {
        selectedItem = sellItem;
    
        sellItemName.text = selectedItem.itemName;
        sellItemDescription.text = selectedItem.description;
        sellItemValue.text = "Value: " + Mathf.FloorToInt(selectedItem.value * 0.5f) + "g";
    }

    public void BuyItem()
    {
        if (selectedItem != null && GameManager.instance.currentGold >= selectedItem.value) {
            GameManager.instance.currentGold -= selectedItem.value;
            GameManager.instance.AddItem(selectedItem.itemName);
        }

        goldText.text = GameManager.instance.currentGold.ToString() + "g";
    }

    public void SellItem()
    {
        if (selectedItem != null) {
            GameManager.instance.currentGold += Mathf.FloorToInt(selectedItem.value * 0.5f);
            GameManager.instance.RemoveItem(selectedItem.itemName);
        }

        goldText.text = Mathf.FloorToInt(selectedItem.value * 0.5f).ToString() + "g";
        ShowSellItems();
    }

    private void ShowSellItems()
    {
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
    }
}
