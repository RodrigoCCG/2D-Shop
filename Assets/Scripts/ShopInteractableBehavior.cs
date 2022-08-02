using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInteractableBehavior : InteractableBehavior
{
    [SerializeField] ShopInventoryScript shopInventory;
    public override void OnInteract(PlayerControl player){
        UIMaster._instance.ScrollTextOnBox(player, base.InteractableText.textList);
        UIMaster._instance.OpenTargetInventory(player, shopInventory.ShopInventoryItems);
    }
}
