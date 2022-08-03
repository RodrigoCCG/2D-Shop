using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] List<Sprite> tutorialImages;
    [SerializeField] Image imageDisplay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayTutorial());
    }

    void Update()
    {
        if(Input.GetKeyDown("escape")){
            Application.Quit();
        }
    }

    IEnumerator PlayTutorial()
    {
        foreach (Sprite image in tutorialImages)
        {
            imageDisplay.sprite = image;
            while(!Input.GetKey("z") && !Input.GetKey("x")){
                yield return new WaitForEndOfFrame();
            }
            if(Input.GetKey("x")){
                break;
            }else{
                yield return new WaitForSeconds(0.15f);
            }
        }
        SceneManager.LoadScene(1);
        yield return null;
    }
}
