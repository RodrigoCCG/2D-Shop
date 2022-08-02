using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClothingSystem : MonoBehaviour
{
    
    [SerializeField] GameObject playerShirt;
    public GameObject PlayerShirt{get{return playerShirt;}}
    [SerializeField] GameObject playerPants;
    public GameObject PlayerPants{get{return playerPants;}}
    [SerializeField] GameObject playerShoes;
    public GameObject PlayerShoes{get{return playerShoes;}}
    //Equiped Shirt
    [SerializeField] InventoryItemScriptable shirtItem;
    public InventoryItemScriptable ShirtItem{get{return shirtItem;}}
    //Equiped pants
    [SerializeField] InventoryItemScriptable pantsItem;
    public InventoryItemScriptable PantsItem{get{return pantsItem;}}
    //Equiped shoes
    [SerializeField] InventoryItemScriptable shoesItem;
    public InventoryItemScriptable ShoesItem{get{return shoesItem;}}
    //List of Items in Inventory
    [SerializeField] List<InventoryItemScriptable> itemsInInventory = new List<InventoryItemScriptable>();
    public List<InventoryItemScriptable> ItemsInInventory {get{return itemsInInventory;}}
    
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
}
