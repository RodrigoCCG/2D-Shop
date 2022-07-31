using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractableBehavior : MonoBehaviour
{
    [SerializeField] List<string> textList;
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI textBox;
    [SerializeField] float textLetterDelay = 0.1f;
    [SerializeField] string skipdialogueButton = "x";
    // Start is called before the first frame update
    public void OnInteract(PlayerMovement player){
        StartCoroutine(ScrollText(player));
    }

    IEnumerator ScrollText(PlayerMovement player)
    {
        dialogueBox.gameObject.SetActive(true);
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
}
