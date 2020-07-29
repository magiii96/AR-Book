using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ShipButton : MonoBehaviour
{
	public GameObject Interaction;


	void Start () {
		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (Click);
	}	

	public void Click(){
		TapToPlaceObject inter = Interaction.GetComponent<TapToPlaceObject>();
		//Interaction.SetActive(false);
		inter.characterOn = false;
		GameObject mobile = GameObject.Find("MobileSingleStickControl");
        GameObject MainMenu = mobile.transform.Find("MainMenu").gameObject;
        MainMenu.transform.Find("DriveBoat").gameObject.SetActive(false);
		MainMenu.transform.Find("ReleaseBoat").gameObject.SetActive(true);
        //mobile.transform.GetChild(2).gameObject.SetActive(true);
		//mobile.transform.GetChild(1).gameObject.SetActive(false);
	}
}
