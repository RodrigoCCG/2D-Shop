using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] bool isInteracting;
    [SerializeField] string inputKey;
    [SerializeField] public string InputKey {get{return inputKey;}}
    private GameObject objectInteraction;
    // Start is called before the first frame update
    void Start()
    {
        inputKey = "z";
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isInteracting){
            transform.Translate(new Vector3(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed,0));
        }
    }

    private void Update() {
        if(Input.GetKeyDown(inputKey) && !isInteracting){
            if(objectInteraction != null){
                isInteracting = true;
                Debug.Log($"Its an {objectInteraction.name}");
                if(objectInteraction.GetComponent<InteractableBehavior>() != null){
                    objectInteraction.GetComponent<InteractableBehavior>().OnInteract(this);
                }
            }else{
                Debug.Log("But nobody came");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        objectInteraction = other.gameObject;
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(objectInteraction == other.gameObject){
            objectInteraction = null;
        }
    }

    public void EnablePlayerMovement(){
        isInteracting = false;
    }
}
