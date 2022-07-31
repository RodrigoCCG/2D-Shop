using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{

    [SerializeField] GameObject playerObject;  
    // Start is called before the first frame update
    
    [SerializeField] Vector3 CameraOffset;

    void Start(){
        CameraOffset = Vector3.back * 10;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = playerObject.transform.position + CameraOffset;
    }

    public void SetNewCameraOffsetAndZoom(Vector3 newOffSet, int newOrthographicSize){
        CameraOffset = newOffSet + Vector3.back * 10;
        GetComponent<Camera>().orthographicSize = newOrthographicSize;
    }
}
