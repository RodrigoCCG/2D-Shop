using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemScriptable", menuName = "2D-Shop/InventoryItemScriptable", order = 0)]
public class InventoryItemScriptable : ScriptableObject {
    //This Scriptable defines all characteristics of an inventory item

    //Prefab that refers to the image displayed in the inventory
    public GameObject InventorySprite;

    //Prefab that refers to the sprite as it appears in the overworld
    public GameObject WorldObject;

    /*
    Clothing type - Value
    Shirt - 0
    Pants - 1
    Shoes - 2    
    */
    public int ClothingType{get{return clothingType;}}
    [SerializeField] int clothingType;

    //Monetary value of the clothing item
    public int moneyValue;

    //Text description of the item
    public string itemDescription;
}