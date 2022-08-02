using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShopInventoryScript", menuName = "2D-Shop/ShopInventoryScript", order = 0)]
public class ShopInventoryScript : ScriptableObject {
    [SerializeField] List<InventoryItemScriptable> shopInventoryItems;
    public List<InventoryItemScriptable> ShopInventoryItems{get{return shopInventoryItems;}}
}

