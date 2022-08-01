using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileControl : MonoBehaviour
{
    private Image thisImage;
    private Sprite defaultSprite;
    private InventoryItemScriptable contents;
    public InventoryItemScriptable Contents {get{return contents;}}


    public void Init(){
        thisImage = GetComponent<Image>();
        defaultSprite = thisImage.sprite;
        contents = null;
    }

    //Change contents and image of this tile
    public void SetNewContents(Image newImage,InventoryItemScriptable newContents){
        thisImage.sprite = newImage.sprite;
        contents = newContents;
    }

    //Remove all contents from tile and reset sprite
    public void EmptyTileContents(){
        thisImage.sprite = defaultSprite;
        contents = null;
    }
}
