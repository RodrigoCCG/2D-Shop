using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBehavior : MonoBehaviour
{
    [SerializeField] TextOnlyScriptable interactableText;
    // Start is called before the first frame update
    public void OnInteract(PlayerControl player){
        UIMaster._instance.ScrollTextOnBox(player,interactableText.textList);
    }
}
