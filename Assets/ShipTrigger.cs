using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipTrigger : MonoBehaviour
{


    void OnCollisionEnter(Collision col){
        if(col.gameObject.name == "character"){
        GameObject mobile = GameObject.Find("MobileSingleStickControl");
        GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        MainMenu.transform.Find("DriveBoat").gameObject.SetActive(true);
        //mobile.transform.GetChild(1).gameObject.SetActive(true);
        }
        
    }
    void OnCollisionExit(Collision col){
        if(col.gameObject.name == "character"){
        GameObject mobile = GameObject.Find("MobileSingleStickControl");
        GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        MainMenu.transform.Find("DriveBoat").gameObject.SetActive(false);
        //mobile.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

}
