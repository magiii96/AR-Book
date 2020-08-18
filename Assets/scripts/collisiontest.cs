using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class collisiontest : MonoBehaviour
{
    [SerializeField]
    private GameObject showobject;

    void OnCollisionEnter(Collision col){
        if(col.gameObject.name == "character"){
            showobject.SetActive(true);
        }
        
    }
    void OnCollisionExit(Collision col){
        if(col.gameObject.name == "character"){
            showobject.SetActive(false);
        }
    }
}
