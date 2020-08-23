using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class collisiontest : MonoBehaviour
{
    [SerializeField]
    private GameObject showobject;
    [SerializeField]
    private Text imagetext;


    void OnTriggerEnter(Collider col){
        if(col.gameObject.name == "character"){
            showobject.SetActive(true);
            imagetext.text = "turn off light";
        }
        
    }
    void OnTriggerExit(Collider col){
        if(col.gameObject.name == "character"){
            showobject.SetActive(false);
            imagetext.text = "";
        }
    }
}
