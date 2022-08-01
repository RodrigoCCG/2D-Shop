using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClothingSystem : MonoBehaviour
{
    
    [SerializeField] GameObject playerShirt;
    [SerializeField] GameObject playerPants;
    [SerializeField] GameObject playerShoes;
    
    //Update clothing Sprites
    public void UpdateClothingSprites(InventoryGridManager inventory)
    {
        playerShirt.GetComponent<SpriteRenderer>().sprite =inventory.ShirtItem.WorldObject.GetComponent<SpriteRenderer>().sprite;
        playerPants.GetComponent<SpriteRenderer>().sprite =inventory.PantsItem.WorldObject.GetComponent<SpriteRenderer>().sprite;
        playerShoes.GetComponent<SpriteRenderer>().sprite =inventory.ShoesItem.WorldObject.GetComponent<SpriteRenderer>().sprite;
    }
}
