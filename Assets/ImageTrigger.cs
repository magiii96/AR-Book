using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageTrigger : MonoBehaviour
{

    private Vector3 cameraPosition;
    private GameObject ARcamera;
    [SerializeField]
    private Text imageTrackedText;

    // Start is called before the first frame update
    void Start()
    {
        ARcamera = GameObject.Find("AR Camera"); 
        cameraPosition = ARcamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float mindis = 1000;
        string minname = "";
        cameraPosition = ARcamera.transform.position;

        foreach (Transform eachChild in transform) {
            float imagedist = Vector3.Distance(eachChild.position, cameraPosition);
            if(imagedist < mindis){
                mindis = imagedist;
                minname = eachChild.name;
            }    
        }
        imageTrackedText.text =  minname;

        foreach (Transform eachChild in transform) {
            GameObject bridgeimg = eachChild.gameObject;
            //MeshRenderer m =bridgeimg.GetComponent<MeshRenderer>();
            if(eachChild.name == minname){
                bridgeimg.SetActive(true);
                //m.enabled = true;
            }else{
                bridgeimg.SetActive(false);
                //m.enabled = false;
            }
        }
        //GameObject shownimage = gameobject.transform.Find(minname);
       
        
     
    }
}
