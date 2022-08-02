using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Player speed on map
    [SerializeField] float moveSpeed;

    [SerializeField] bool isInteracting;
    //[SerializeField] bool menuOpen;

    //Substitute for player settings later
    [SerializeField] string inputKey;
    [SerializeField] string menuKey;

    [SerializeField] public string InputKey {get{return inputKey;}}
    //[SerializeField] public bool MenuOpen {get{return menuOpen;}}

    //Holder for Interaction
    private GameObject objectInteraction;

    //InventoryGridManager Reference
    private InventoryGridManager inventoryGrid;

    // Start is called before the first frame update
    void Start()
    {
        //Substitute for player settings later
        inputKey = "z";
        menuKey = "c";
    }

    // Update is called once per frame
    void FixedUpdate(){
        //Move player according to directional inputs
        if(!isInteracting && !UIMaster._instance.IsAMenuOpen){
            transform.Translate(new Vector3(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed,0));
        }
    }

    private void Update(){
        //Interaction System
        if(Input.GetKeyDown(inputKey) && !isInteracting && !UIMaster._instance.IsATextBoxOpen && !UIMaster._instance.IsAMenuOpen){
            if(objectInteraction != null){
                InteractWithObject();
            }else{
                Debug.Log("But nobody came");
            }
        }
        //Opening Menu System
        if(Input.GetKeyDown(menuKey) && !UIMaster._instance.IsATextBoxOpen){
            if(UIMaster._instance.IsAMenuOpen){
                //menuOpen = false;
                UIMaster._instance.ClosePlayerInventory();
            }else{
                //menuOpen = true;
                UIMaster._instance.OpenPlayerInventory(this);
            }
        }
    }

    public void EnablePlayerMovement(){
        //Enable player's ability to move
        isInteracting = false;
    }

    void InteractWithObject(){
        //Activate functions of the object last collided with
        isInteracting = true;
        Debug.Log($"Its an {objectInteraction.name}");
        if(objectInteraction.GetComponent<InteractableBehavior>() != null){
            objectInteraction.GetComponent<InteractableBehavior>().OnInteract(this);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Identify what object was last touched for interaction
        objectInteraction = other.gameObject;
    }

    private void OnCollisionExit2D(Collision2D other) {
        //Remove reference when distancing from objects
        if(objectInteraction == other.gameObject){
            objectInteraction = null;
        }
    }

}
