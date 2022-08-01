using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIMaster : MonoBehaviour
{
    //Singleton Instance for UI Controller
    public static UIMaster _instance;

    //Storage of UI elements for Display
    [SerializeField] GameObject dialogueBox;
    [SerializeField] GameObject inventoryDisplay;

    private bool isAMenuOpen;
    public bool IsAMenuOpen {get{return isAMenuOpen;}}
    private bool isATextBoxOpen;

    //Storage of camera instance
    [SerializeField] GameObject cameraGO;
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] float textLetterDelay = 0.05f;
    [SerializeField] string skipdialogueButton = "x";
    
    //Set global instance of UIMaster
    void Awake()
    {
        _instance = this;
    }

    // Start is called before the first frame update
    void Start(){
        CloseInventory();
        isAMenuOpen = false;
        isATextBoxOpen = false;
    }

    //Start Text scroll routine
    public void ScrollTextOnBox(PlayerControl player, List<string> textList){
        isATextBoxOpen = true;
        StartCoroutine(ScrollText(player, textList));
        StartCoroutine(WaitForActions(player));
    }

    //Routine to scroll text, pass dialogue boxes
    IEnumerator ScrollText(PlayerControl player,List<string> textList)
    {
        dialogueBox.SetActive(true);
        foreach (string text in textList)
        {
            for(int i = 0; i <= text.Length; i++){
                if(Input.GetKey(skipdialogueButton) || (Input.GetKey(player.InputKey) && i>1)){
                    i = text.Length;
                }
                textBox.text = text.Substring(0,i);
                yield return new WaitForSeconds(textLetterDelay);
            }
            while(Input.GetKey(player.InputKey)){
                yield return new WaitForEndOfFrame();
            }
            bool waitingForInput = true;
            while(waitingForInput){
                yield return new WaitForEndOfFrame();
                if(Input.GetKey(player.InputKey) || Input.GetKey(skipdialogueButton)){
                    waitingForInput = false;
                }
            }
        }
        isATextBoxOpen = false;
        player.EnablePlayerMovement();
        dialogueBox.gameObject.SetActive(false);
        yield return null;
    }

    //Function that renables player movement once All menus and text boxes are closed
    IEnumerator WaitForActions(PlayerControl player){
        while(isAMenuOpen || isATextBoxOpen){
            yield return new WaitForEndOfFrame();
        }
        player.EnablePlayerMovement();
    }

    //Function to open inventory, change camera position and zoom in
    public void OpenInventory(){
        isAMenuOpen = true;
        inventoryDisplay.SetActive(true);
        cameraGO.GetComponent<CameraBehavior>().SetNewCameraOffsetAndZoom(Vector3.right * 2.5f, 2);
    }

    //Function to close inventory, change camera position and zoom out
    public void CloseInventory(){
        isAMenuOpen = false;
        inventoryDisplay.SetActive(false);
        cameraGO.GetComponent<CameraBehavior>().SetNewCameraOffsetAndZoom(new Vector3(0,0,0), 5);
    }
}
