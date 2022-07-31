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

    //Storage of camera instance
    [SerializeField] GameObject cameraGO;
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] float textLetterDelay = 0.1f;
    [SerializeField] string skipdialogueButton = "x";
    // Start is called before the first frame update
    
    void Start(){
        CloseInventory();
    }

    public void ScrollTextOnBox(PlayerControl player, List<string> textList){
        StartCoroutine(ScrollText(player, textList));
    }

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
                yield return new WaitForSeconds(0.01f);
            }
            bool waitingForInput = true;
            while(waitingForInput){
                yield return new WaitForSeconds(0.01f);
                if(Input.GetKey(player.InputKey) || Input.GetKey(skipdialogueButton)){
                    waitingForInput = false;
                }
            }
        }
        dialogueBox.gameObject.SetActive(false);
        player.EnablePlayerMovement();
        yield return null;
    }

    public void OpenInventory(){
        inventoryDisplay.SetActive(true);
        cameraGO.GetComponent<CameraBehavior>().SetNewCameraOffsetAndZoom(Vector3.right * 2.5f, 2);
    }

    public void CloseInventory(){
        inventoryDisplay.SetActive(false);
        cameraGO.GetComponent<CameraBehavior>().SetNewCameraOffsetAndZoom(new Vector3(0,0,0), 5);
    }
}
