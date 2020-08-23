using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class bridgeVideo : MonoBehaviour
{

    // private ARSessionOrigin arOrigin;
    [SerializeField]
    private Text imageTrackedText;

    // void Start(){
    //     arOrigin = FindObjectOfType<ARSessionOrigin>();
    // }
    void Start(){
        this.transform.Find("VideoCube").gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.name == "character"){
            this.transform.Find("VideoCube").gameObject.SetActive(true);
            imageTrackedText.text = "find";
            
        }
    }
        
    void OnTriggerExit(Collider col){
        if(col.gameObject.name == "character"){
            imageTrackedText.text = "no";
            this.transform.Find("VideoCube").gameObject.SetActive(false);
      
        }
    }
}
