using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBehavior : MonoBehaviour
{
    [SerializeField] TextOnlyScriptable interactableText;
    public TextOnlyScriptable InteractableText{get{return interactableText;}}
    // Start is called before the first frame update
    public virtual void OnInteract(PlayerControl player){
        UIMaster._instance.ScrollTextOnBox(player,interactableText.textList);
    }
}
