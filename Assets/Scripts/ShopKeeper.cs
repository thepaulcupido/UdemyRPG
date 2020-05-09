using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopKeeper : MonoBehaviour
{

    private bool canOpenShop;
    public string[] itemsForSale = new string[40];

    void OnTriggerEnter2D(Collider2D other)
    {
        canOpenShop = other.tag == "Player";
    }
    void Start()
    {
        if (canOpenShop && !Shop.instance.shopMenu.active) {
            Shop.instance.itemsForSale = itemsForSale;
            Shop.instance.OpenShop();
        }        
    }

    void Update()
    {
        if (PlayerController.instance.movementEnabled && canOpenShop && !Shop.instance.shopMenu.active) {
            Shop.instance.itemsForSale = itemsForSale;
            Shop.instance.OpenShop();
        }        
    }
}
