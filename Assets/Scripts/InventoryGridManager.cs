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

    [SerializeField] bool isInShop;
    [SerializeField] bool isSellingItems;
    private Dictionary<Vector2, TileControl> TileDict = new Dictionary<Vector2, TileControl>();
    [SerializeField] List<InventoryItemScriptable> itemsInInventory = new List<InventoryItemScriptable>();
    [SerializeField] List<InventoryItemScriptable> shopInventory = new List<InventoryItemScriptable>();

    //Reference and PlayerClothes
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
        if(isInShop){
            isSellingItems = true;
        }
        LoadInventory(0);
        StartCoroutine(InventoryControl());
    }

    public void OpenInventory(List<InventoryItemScriptable> inventoryToDisplay){
        shopInventory = inventoryToDisplay;
        OpenInventory();
    }
    public void OpenInventory(){
        itemsInInventory = shopInventory;
        isInShop = true;
        isSellingItems = false;
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

    //Menu navigation for equipping clothes
    IEnumerator InventoryControl(){
        highlightPos = new Vector2(0,0);
        highlight.transform.localPosition = TileDict[highlightPos].transform.localPosition + Vector3.forward;
        while(UIMaster._instance.IsATextBoxOpen){
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.15f);
        while(UIMaster._instance.IsAMenuOpen){
            //Input of the highlight
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
                //Move the highlight
                if(verticalInput != 0 || horizontalInput !=0){
                    highlightPos += new Vector2(verticalInput, horizontalInput);
                    highlight.transform.localPosition = TileDict[highlightPos].transform.localPosition + Vector3.forward;
                    yield return new WaitForSeconds(0.15f);
                }else{
                    //Equip, Purchase or Sell Input
                    if(Input.GetKey("z")){
                        InventoryItemScriptable itemToWear = TileDict[highlightPos].Contents;
                        if(itemToWear != null){
                            if(!isInShop){
                                if(itemToWear.ClothingType == 0){
                                    playerClothes.SwapShirt(itemToWear);
                                }
                                if(itemToWear.ClothingType == 1){
                                    playerClothes.SwapPants(itemToWear);
                                }
                                if(itemToWear.ClothingType == 2){
                                    playerClothes.SwapShoes(itemToWear);
                                }
                                LoadInventory(0);
                            }else{
                                int transactionSucceeded = -1;
                                if(isSellingItems){
                                    transactionSucceeded = playerClothes.MakeSale(itemToWear);
                                }else{
                                    transactionSucceeded = playerClothes.MakePurchase(itemToWear);
                                }
                                if(isSellingItems){
                                    switch(transactionSucceeded){
                                    case 0:
                                        UIMaster._instance.ScrollTextOnBox(playerControl,$"You got ${itemToWear.moneyValue}!");
                                        LoadInventory(0);
                                    break;
                                    case 1:
                                        UIMaster._instance.ScrollTextOnBox(playerControl,"You can't sell what you're wearing!");
                                    break;
                                    case 2:
                                        UIMaster._instance.ScrollTextOnBox(playerControl,"You can't sell what you don't own!");
                                    break;
                                    default:
                                        UIMaster._instance.ScrollTextOnBox(playerControl,"If you're reading this, I screwed up.");
                                    break;
                                    }
                                }else{
                                    switch(transactionSucceeded){
                                    case 0:
                                        UIMaster._instance.ScrollTextOnBox(playerControl,"Thank you for your purchase!");
                                    break;
                                    case 1:
                                        UIMaster._instance.ScrollTextOnBox(playerControl,"Looks like you're short on cash!");
                                    break;
                                    case 2:
                                        UIMaster._instance.ScrollTextOnBox(playerControl,"You already own that!");
                                    break;
                                    default:
                                        UIMaster._instance.ScrollTextOnBox(playerControl,"If you're reading this, I screwed up.");
                                    break;
                                    }
                                }
                                while(UIMaster._instance.IsATextBoxOpen) yield return new WaitForEndOfFrame();
                            }
                        }
                        yield return new WaitForSeconds(0.15f);
                    }else{
                        //Description Input
                        if(Input.GetKey("x")){
                            yield return new WaitForSeconds(0.15f);
                            InventoryItemScriptable itemToWear = TileDict[highlightPos].Contents;
                            UIMaster._instance.ScrollTextOnBox(playerControl, itemToWear.itemDescription);
                            while(UIMaster._instance.IsATextBoxOpen) yield return new WaitForEndOfFrame();
                            yield return new WaitForSeconds(0.15f);
                        }
                        if(Input.GetKey("space")){
                            if(isInShop){
                                if(isSellingItems){
                                    OpenInventory();
                                }else{
                                    OpenPlayerInventory();
                                }
                                break;
                            }
                            yield return new WaitForSeconds(0.15f);
                        }
                    }
                } 
            }       
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDisable() {
        isInShop = false;
        isInventoryLoaded = false;
    }
}
