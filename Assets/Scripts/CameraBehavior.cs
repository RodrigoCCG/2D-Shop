using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    [SerializeField] GameObject playerObject;  
    // Start is called before the first frame update
    

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerObject.transform.position + Vector3.back * 10; 
    }
}
