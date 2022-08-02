using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryGridManager : MonoBehaviour
{
    //Inventory Grid Configuration
    [SerializeField] int gridHeight = 4;
    [SerializeField] int gridWidth = 6;

    [SerializeField] float xAnchor = -220;
    [SerializeField] float yAnchor = -165;
    [SerializeField] float spacing = 88.5f;

    //Prefab for Inventory Tile
    [SerializeField] GameObject TilePrefab;
    //Prefab for Inventory Highlight
    [SerializeField] GameObject InventoryHighlightPrefab;
    //Reference to Player
    [SerializeField] GameObject playerGO;

    private GameObject highlight;
    private Vector2 highlightPos;
    private bool isInventoryLoaded;
    private bool showPlayerEquipped = false;
    private Dictionary<Vector2, TileControl> TileDict = new Dictionary<Vector2, TileControl>();
    [SerializeField] List<InventoryItemScriptable> itemsInInventory = new List<InventoryItemScriptable>();

    private PlayerClothingSystem playerClothes;
    private PlayerControl playerControl;



    // Start is called before the first frame update
    void Start()
    {
        highlightPos = new Vector2(0,0);
        highlight = Instantiate(InventoryHighlightPrefab, new Vector3(0,0,0), Quaternion.identity, transform.parent);
        for(int i = 0 ; i < gridHeight; i++){
            for(int j = 0; j < gridWidth; j++){
                float xPosTile = xAnchor + (j * spacing);
                float yPosTile = yAnchor + ((gridHeight - (i+1)) * spacing);
                Vector3 newTilePos = new Vector3(xPosTile,yPosTile,-2);
                GameObject newTile = Instantiate(TilePrefab, new Vector3(0,0,0), Quaternion.identity, transform.parent);
                newTile.transform.localPosition = newTilePos;
                newTile.name = $"InvTile:{i}-{j}";
                TileDict.Add(new Vector2(i,j), newTile.GetComponent<TileControl>());
                TileDict[new Vector2(i,j)].Init();
            }
        }
        playerClothes = playerGO.GetComponent<PlayerClothingSystem>();
        playerControl = playerGO.GetComponent<PlayerControl>();
        highlight.transform.localPosition = TileDict[highlightPos].transform.localPosition + Vector3.forward;
        isInventoryLoaded = false;
        //Failsafe if Worn clothes are not in the inventory
        if(!playerClothes.ItemsInInventory.Contains(playerClothes.ShirtItem)) playerClothes.ItemsInInventory.Add(playerClothes.ShirtItem);
        if(!playerClothes.ItemsInInventory.Contains(playerClothes.PantsItem)) playerClothes.ItemsInInventory.Add(playerClothes.PantsItem);
        if(!playerClothes.ItemsInInventory.Contains(playerClothes.ShoesItem)) playerClothes.ItemsInInventory.Add(playerClothes.ShoesItem);
    }

    public void OpenPlayerInventory(){
        itemsInInventory = playerClothes.ItemsInInventory;
        showPlayerEquipped = true;
        LoadInventory(0);
        StartCoroutine(InventoryControl());
    }

    public void OpenInventory(List<InventoryItemScriptable> inventoryToDisplay){
        itemsInInventory = inventoryToDisplay;
        showPlayerEquipped = false;
        LoadInventory(0);
        StartCoroutine(InventoryControl());
    }

    //Load the items into the tiles in the inventory
    void LoadInventory(int start){
        int endIndex = start+21 < itemsInInventory.Count ? start+21 : itemsInInventory.Count;
        List<InventoryItemScriptable> displayedInventory = itemsInInventory.GetRange(start, endIndex);
        TileControl tileToFill = TileDict[new Vector2(0,0)];
        int rowIndex = 0;
        int columnIndex = 0;
        for(rowIndex = 0; rowIndex < gridHeight; rowIndex++){
            for(columnIndex = 0; columnIndex < gridWidth; columnIndex++){
                tileToFill = TileDict[new Vector2(rowIndex,columnIndex)];
                tileToFill.EmptyTileContents();
            }
        }
        if(showPlayerEquipped){
            tileToFill = TileDict[new Vector2(0,0)];
            tileToFill.SetNewContents(playerClothes.ShirtItem.InventorySprite.GetComponent<Image>(),playerClothes.ShirtItem);
            tileToFill = TileDict[new Vector2(0,1)];
            tileToFill.SetNewContents(playerClothes.PantsItem.InventorySprite.GetComponent<Image>(),playerClothes.PantsItem);
            tileToFill = TileDict[new Vector2(0,2)];
            tileToFill.SetNewContents(playerClothes.ShoesItem.InventorySprite.GetComponent<Image>(),playerClothes.ShoesItem);
            rowIndex = 0;
            columnIndex = 3;
        }else{
            rowIndex = 0;
            columnIndex = 0;
        }
        foreach(InventoryItemScriptable item in displayedInventory){
            if(item != playerClothes.ShirtItem && item != playerClothes.PantsItem && item != playerClothes.ShoesItem){
                tileToFill = TileDict[new Vector2(rowIndex,columnIndex)];
                tileToFill.SetNewContents(item.InventorySprite.GetComponent<Image>(),item);
                columnIndex++;
                if(columnIndex >= gridWidth){
                    columnIndex = 0;
                    rowIndex++;
                    if(rowIndex >= gridHeight){
                        break;
                    }
                }
            }
        }
        isInventoryLoaded = true;
    }

    //Menu navigation
    IEnumerator InventoryControl(){
        highlightPos = new Vector2(0,0);
        highlight.transform.localPosition = TileDict[highlightPos].transform.localPosition + Vector3.forward;
        while(UIMaster._instance.IsATextBoxOpen){
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.15f);
        while(UIMaster._instance.IsAMenuOpen){
            int horizontalInput = 0;
            if(Input.GetAxis("Horizontal") < -0.1f || Input.GetAxis("Horizontal") > 0.1f){
                horizontalInput = Input.GetAxis("Horizontal") < 0 ? -1 : 1;
                if(highlightPos.y + horizontalInput > gridWidth-1 || highlightPos.y + horizontalInput < 0){
                    horizontalInput = 0;
                }
            }
            int verticalInput = 0;
            if(Input.GetAxis("Vertical") > 0.1f || Input.GetAxis("Vertical") < -0.1f){
                verticalInput = Input.GetAxis("Vertical") > 0.1f ? -1 : 1;
                if(highlightPos.x + verticalInput < 0 || highlightPos.x + verticalInput > gridHeight-1){
                    verticalInput = 0;
                }
            }
            if(!UIMaster._instance.IsATextBoxOpen){
                if(verticalInput != 0 || horizontalInput !=0){
                    highlightPos += new Vector2(verticalInput, horizontalInput);
                    highlight.transform.localPosition = TileDict[highlightPos].transform.localPosition + Vector3.forward;
                    yield return new WaitForSeconds(0.15f);
                }else{
                    if(Input.GetKey("z")){
                        InventoryItemScriptable itemToWear = TileDict[highlightPos].Contents;
                        if(itemToWear != null){
                            if(itemToWear.ClothingType == 0){
                                playerClothes.SwapShirt(itemToWear);
                            }
                            if(itemToWear.ClothingType == 1){
                                playerClothes.SwapPants(itemToWear);
                            }
                            if(itemToWear.ClothingType == 2){
                                playerClothes.SwapShoes(itemToWear);
                            }
                        }
                        LoadInventory(0);
                        yield return new WaitForSeconds(0.15f);
                    }
                } 
            }       
            yield return new WaitForEndOfFrame();
        }
    }
    /*
    //Change equiped shirt
    void SwapShirt(InventoryItemScriptable itemToWear){
        if(shirtItem != itemToWear){
            InventoryItemScriptable itemHolder = shirtItem;
            shirtItem = itemToWear;
            playerGO.GetComponent<PlayerClothingSystem>().UpdateClothingSprites(this);
        }
    }

    //Change equiped pants
    void SwapPants(InventoryItemScriptable itemToWear){
        if(pantsItem != itemToWear){
            InventoryItemScriptable itemHolder = pantsItem;
            pantsItem = itemToWear;
            playerGO.GetComponent<PlayerClothingSystem>().UpdateClothingSprites(this);
        }
    }

    //Change equiped shoes
    void SwapShoes(InventoryItemScriptable itemToWear){
        if(shoesItem != itemToWear){
            InventoryItemScriptable itemHolder = shoesItem;
            shoesItem = itemToWear;
            playerGO.GetComponent<PlayerClothingSystem>().UpdateClothingSprites(this);
        }
    }
    */
    private void OnDisable() {
        isInventoryLoaded = false;
    }
}
