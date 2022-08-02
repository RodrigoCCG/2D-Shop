using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClothingSystem : MonoBehaviour
{
    //Equipped items Game Object Reference
    [SerializeField] GameObject playerShirt;
    public GameObject PlayerShirt{get{return playerShirt;}}
    [SerializeField] GameObject playerPants;
    public GameObject PlayerPants{get{return playerPants;}}
    [SerializeField] GameObject playerShoes;
    public GameObject PlayerShoes{get{return playerShoes;}}

    //Equiped Shirt Data
    [SerializeField] InventoryItemScriptable shirtItem;
    public InventoryItemScriptable ShirtItem{get{return shirtItem;}}
    //Equiped pants Data
    [SerializeField] InventoryItemScriptable pantsItem;
    public InventoryItemScriptable PantsItem{get{return pantsItem;}}
    //Equiped shoes Data
    [SerializeField] InventoryItemScriptable shoesItem;
    public InventoryItemScriptable ShoesItem{get{return shoesItem;}}

    //List of Items in Inventory
    [SerializeField] List<InventoryItemScriptable> itemsInInventory = new List<InventoryItemScriptable>();
    public List<InventoryItemScriptable> ItemsInInventory {get{return itemsInInventory;}}
    
    [SerializeField] int playerMoney = 0;
    public int PlayerMoney{get{return playerMoney;}}
    
    //Update clothing Sprites
    public void UpdateClothingSprites()
    {
        playerShirt.GetComponent<SpriteRenderer>().sprite = ShirtItem.WorldObject.GetComponent<SpriteRenderer>().sprite;
        playerPants.GetComponent<SpriteRenderer>().sprite = PantsItem.WorldObject.GetComponent<SpriteRenderer>().sprite;
        playerShoes.GetComponent<SpriteRenderer>().sprite = ShoesItem.WorldObject.GetComponent<SpriteRenderer>().sprite;
    }
    
    //Change equiped shirt
    public void SwapShirt(InventoryItemScriptable itemToWear){
        if(shirtItem != itemToWear){
            InventoryItemScriptable itemHolder = shirtItem;
            shirtItem = itemToWear;
            UpdateClothingSprites();
        }
    }

    //Change equiped pants
    public void SwapPants(InventoryItemScriptable itemToWear){
        if(pantsItem != itemToWear){
            InventoryItemScriptable itemHolder = pantsItem;
            pantsItem = itemToWear;
            UpdateClothingSprites();
        }
    }

    //Change equiped shoes
    public void SwapShoes(InventoryItemScriptable itemToWear){
        if(shoesItem != itemToWear){
            InventoryItemScriptable itemHolder = shoesItem;
            shoesItem = itemToWear;
            UpdateClothingSprites();
        }
    }

    public int GainMoney(int moneyGained){
        playerMoney += moneyGained;
        return playerMoney;
    }

    public int LoseMoney(int moneyLost){
        playerMoney -= moneyLost;
        return playerMoney;
    }

    public int MakePurchase(InventoryItemScriptable itemToBuy){
        if(!(playerMoney >= itemToBuy.moneyValue)){
            return 1;
        }else if(ItemsInInventory.Contains(itemToBuy)){
            return 2;
        }else{
            LoseMoney(itemToBuy.moneyValue);
            itemsInInventory.Add(itemToBuy);
            return 0;
        }
    }

    public int MakeSale(InventoryItemScriptable itemToBuy){
        if((itemToBuy == shirtItem || itemToBuy == pantsItem || itemToBuy == shoesItem)){
            return 1; 
        }else if(!ItemsInInventory.Contains(itemToBuy)){
            return 2; 
        }else{
            GainMoney(itemToBuy.moneyValue);
            itemsInInventory.Remove(itemToBuy);
            return 0;
        }
    }
}
