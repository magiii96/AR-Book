using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipTrigger : MonoBehaviour
{
    [SerializeField]
    private Button DriveButton;

    void OnCollisionEnter(Collision col){
        if(col.gameObject.name == "character"){
        // GameObject mobile = GameObject.Find("MobileSingleStickControl");
        // GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        // GameObject BoatControl = MainMenu.transform.Find("BoatControl").gameObject;
        // BoatControl.transform.Find("DriveBoat").gameObject.SetActive(true);
        //mobile.transform.GetChild(1).gameObject.SetActive(true);
        DriveButton.gameObject.SetActive(true);
        }
        
    }
    void OnCollisionExit(Collision col){
        if(col.gameObject.name == "character"){
        // GameObject mobile = GameObject.Find("MobileSingleStickControl");
        // GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        // GameObject BoatControl = MainMenu.transform.Find("BoatControl").gameObject;
        // BoatControl.transform.Find("DriveBoat").gameObject.SetActive(false);
        //mobile.transform.GetChild(1).gameObject.SetActive(false);
        DriveButton.gameObject.SetActive(false);
        }
    }

}
